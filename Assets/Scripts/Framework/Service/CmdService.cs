using System.Collections.Generic;
using BEBE.Framework.Event;
using BEBE.Engine.Math;
using BEBE.Engine.Service.Cmd;
using BEBE.Engine.Service.Net;
using BEBE.Framework.Component;
using BEBE.Framework.Managers;
using BEBE.Framework.Service.Net;
using UnityEngine;
using BEBE.Framework.Service.Net.Msg;
using BEBE.Framework.Module;
using BEBE.Game.Inputs;

namespace BEBE.Framework.Service
{
    public class ClientCmdService : CmdService
    {
        public int TickSync => tick_sync;
        public void SyncTick(int tick)
        {
            tick_sync = tick;
        }
        private int tick_sync = -1;//同步的帧数
        private FrameMgr frameMgr => MgrsContainer.GetMgr<FrameMgr>();
        private EntityMgr entityMgr => MgrsContainer.GetMgr<EntityMgr>();
        private CommonStatusMgr commonStatusMgr => MgrsContainer.GetMgr<CommonStatusMgr>();
        private PlayerInputDataCache m_inputs_push = new PlayerInputDataCache();  // 发送给服务器的
        public TickInputsRollbackableCache InputsRollbackable => m_inputs_rollbackable;
        private TickInputsRollbackableCache m_inputs_rollbackable = new TickInputsRollbackableCache(); // 回滚缓存
        //记录输入
        //记录的数量约等于 客户端当前帧数 - 客户端收到服务端同步的帧数
        public void RecordInput()
        {
            TickInputs tick_inputs_local = new TickInputs(frameMgr.Tick, commonStatusMgr.Members);

            //获取当前帧的所有输入,存入本地与网络返回的进行比对
            //获取当前帧你的输入，准备上传给服务器
            for (byte i = 0; i < commonStatusMgr.Members; i++)
            {
                if (entityMgr.TryGetPlayer(i, out Entity entity))
                {
                    if (i == commonStatusMgr.ActorId)
                    {
                        You you = entity as You;
                        var cmd = you.RecordCmd();
                        tick_inputs_local.Put(cmd);
                        m_inputs_push.Put(new PlayerInputData(frameMgr.Tick, cmd));
                    }
                    else
                    {
                        Partner partner = entity as Partner;
                        tick_inputs_local.Put(partner.RecordCmd());
                    }
                }
            }
            m_inputs_rollbackable.TryAddRollbackableCmd(tick_inputs_local);
           
        }

        // 上传指令
        public void push_cmd()
        {
            Dispatchor.Dispatch(EventCode.CALL_PUSH_CMD_METHOD, m_inputs_push.GetBytes());
        }

        private TickInputsCache m_inputs_pull = new TickInputsCache();
        protected void EVENT_PULL_CMD(object param)
        {
            EventMsg msg = (EventMsg)param;

            lock (m_inputs_pull)
            {
                m_inputs_pull.PutBytes(msg.Content);

                // Debug.Log(m_inputs_pull.ToString());
                while (m_inputs_pull.TryGet(out TickInputs inputs))
                {
                    if (inputs.Tick < tick_sync) continue;
                    if (m_inputs_rollbackable.TryGetRollbackableCmd(inputs.Tick, out TickInputs local_inputs))
                    {
                        if (!inputs.Equals(local_inputs))
                        {
                            //add to rollback
                            m_inputs_rollbackable.PutSyncCmd(inputs);
                        }
                        else
                        {
                            tick_sync = inputs.Tick;
                            m_inputs_rollbackable.TryRemoveRollbackableCmd(tick_sync, out TickInputs local);
                        }
                    }
                }
            }
        }
    }

    public class ServerCmdService : CmdService
    {
        Dictionary<int, Dictionary<byte, PlayerInput>> dict = new Dictionary<int, Dictionary<byte, PlayerInput>>();
        TickInputsCache inputs_cache = new TickInputsCache();
        private void EVENT_PUSH_CMD(object param)
        {
            EventMsg msg = (EventMsg)param;
            int channel_id = msg.Id;

            PlayerInputDataCache cache = new PlayerInputDataCache();
            cache.PutBytes(msg.Content);

            while (cache.TryGet(out PlayerInputData data))
            {
                int tick = data.Tick;
                PlayerInput input = data.PlayerInput;
                if (dict.ContainsKey(tick))
                {
                    if (dict[tick] == null)
                    {
                        dict[tick] = new Dictionary<byte, PlayerInput>();
                    }
                    if (!dict[tick].ContainsKey(input.actorId))
                    {
                        dict[tick].Add(input.actorId, input);
                    }
                }
                else
                {
                    dict.Add(tick, new Dictionary<byte, PlayerInput>());
                    dict[tick].Add(input.actorId, input);
                }
                if (dict[tick].Count == Constant.ROOM_CAPICITY)
                {
                    //将这一帧加入的发送队列
                    TickInputs inputs = new TickInputs(tick, Constant.ROOM_CAPICITY);
                    foreach (var element in dict[tick].Values)
                    {
                        inputs.Put(element);
                    }
                    inputs_cache.Put(inputs);
                    dict.Remove(tick);
                }
            }

            //返回客户端
            if (inputs_cache.Count > 0)
            {
                Dispatchor.Dispatch(EventCode.CALL_PULL_CMD_METHOD, new EventMsg(EventCode.PULL_CMD, inputs_cache.GetBytes(), channel_id));
            }

        }

    }
}