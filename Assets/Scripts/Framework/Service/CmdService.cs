using System.Collections.Generic;
using BEBE.Engine.Event;
using BEBE.Engine.Math;
using BEBE.Engine.Service.Cmd;
using BEBE.Engine.Service.Net;
using BEBE.Framework.Component;
using BEBE.Framework.Managers;
using UnityEngine;

namespace BEBE.Framework.Service
{
    public class ClientCmdService : CmdService
    {
        private byte actorId; // 范围 1 ~ 10号位
        public ClientCmdService(NetService netService) : base(netService)
        {
            
        }

        public int tick_sync = -1;//同步的帧数
        private FrameMgr frameMgr => MgrsContainer.GetMgr<FrameMgr>();
        PlayerInputs m_inputs2send = new PlayerInputs();
        PlayerInputs m_inputs_local = new PlayerInputs();
        public PlayerInputs Inputs => m_inputs_local;
        Queue<PlayerInput> m_inputs_rollback = new Queue<PlayerInput>();
        //记录输入
        //记录的数量约等于 客户端当前帧数 - 客户端收到服务端同步的帧数
        public void RecordInput()
        {
            rollback_cmd();
            //获取移动的指令
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            //装载
            PlayerInput input_tick = new PlayerInput();
            m_inputs_local.tick = m_inputs2send.tick = input_tick.tick = frameMgr.Tick;
            input_tick.actorId = actorId;
            input_tick.x = x.ToLFloat();
            input_tick.y = y.ToLFloat();
            m_inputs_local.put(input_tick);
            if (m_inputs2send.put(input_tick))
                send_cmd();
        }

        private EntityMgr entityMgr => MgrsContainer.GetMgr<EntityMgr>();

        protected void rollback_cmd()
        {
            if (entityMgr.TryGetPlayer(0, out Entity p))
            {
                var player = ((PlayerEntity)p);
                // Step.1 ROLLBACK FUNTION TODO
                for (int i = frameMgr.Tick; i > tick_sync; i--)
                {
                    if (m_inputs_local.get(i, out PlayerInput input))
                    {
                        player.RollbackCmd(input);
                    }
                }
                // Step.2 Execute ensure cmd;
                while (m_inputs_rollback.Count > 0)
                {
                    var input_rollback = m_inputs_rollback.Dequeue();
                    tick_sync = input_rollback.tick;

                    player.ExecuteCmd(input_rollback);

                }
            }
        }

        protected void send_cmd()
        {
            m_netservice.Send(new Packet(new EventMsg(EventCode.ON_RECV_INPUT, m_inputs2send.GetBytes(), ((TCPClientService)m_netservice).Id)));
        }

        private void sync_cmd(PlayerInputs inputs)
        {
            foreach (var input in inputs.Inputs.Values)
            {
                if (input.tick > tick_sync) //同步
                {
                    //server response 与 local 做比较
                    // 如果比较的一帧的操作相同吗，则不回滚,否则回滚
                    if (m_inputs_local.get(input.tick, out PlayerInput input_local))
                    {
                        // BEBE.Engine.Logging.Debug.Log("m_inputs_local.get tick");
                        if (input_local.Equals(input))
                        {
                            //此帧确认正确
                            tick_sync = input.tick;

                            if (input_local.executed)
                            {
                                m_inputs_local.delete(tick_sync);
                            }
                            // BEBE.Engine.Logging.Debug.Log("Ensure tick");
                        }
                        else
                        {
                            //rollback to server tick 回滚
                            // BEBE.Engine.Logging.Debug.Log($"rollback tick  {input.ToString()}  {input_local.ToString()}");
                            m_inputs_rollback.Enqueue(input);
                        }
                    }
                    else
                    {
                        // BEBE.Engine.Logging.Debug.Log("m_inputs_local.get tick fail");
                    }
                }
                else
                {
                    // BEBE.Engine.Logging.Debug.Log($"input.tick {input.tick}  tick_sync {tick_sync}");
                    if (m_inputs_local.get(input.tick, out PlayerInput input_local))
                    {
                        if (input_local.executed)
                        {
                            m_inputs_local.delete(tick_sync);
                        }
                    }
                }
                m_inputs2send.delete(input.tick);
            }
        }

        protected void EVENT_ON_SYNC_CMD(object param)
        {
            // BEBE.Engine.Logging.Debug.Log("EVENT_ON_RECV_INPUT");
            EventMsg msg = (EventMsg)param;
            int id = msg.Id;

            PlayerInputs p_inputs = new PlayerInputs();
            p_inputs.DecodeBytes(msg.Content);

            sync_cmd(p_inputs);
        }


    }

    public class ServerCmdService : CmdService
    {
        public ServerCmdService(NetService netService) : base(netService)
        {

        }

        public void SendCmd(Cmd binput)
        {

        }

        protected void EVENT_ON_RECV_INPUT(object param)
        {
            // BEBE.Engine.Logging.Debug.Log("EVENT_ON_RECV_INPUT");
            EventMsg msg = (EventMsg)param;
            int id = msg.Id;

            PlayerInputs p_inputs = new PlayerInputs();
            p_inputs.DecodeBytes(msg.Content);

            // BEBE.Engine.Logging.Debug.Log(p_inputs.ToString());

            m_netservice.Send(new Packet(new EventMsg(EventCode.ON_SYNC_CMD, msg.Content)));
            //如果收到所有的cmds ,分发给client
            // m_inputs.put(id, msg.Content);
            //并清空dict


        }
    }
}