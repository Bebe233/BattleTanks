using BEBE.Engine.Service.Net;
using BEBE.Framework.Event;
using BEBE.Framework.Module;
using BEBE.Framework.Service.Net.Msg;
using BEBE.Framework.Utils;
using BEBE.Engine.Logging;
using BEBE.Engine.Service.Net.Utils;
using BEBE.Framework.Managers;
using BEBE.Engine.Math;
using System;

namespace BEBE.Framework.Service.Net
{
    public class UClientService : ClientService
    {
        public UClientService(string ip_address, int port) : base(ip_address, port)
        {
            m_channel = new UChannel(ip_address, port);
        }

        protected override void register_events()
        {
            Dispatchor.Register(this, Constant.EVENT_PREFIX);

            OnConnected += ping;

            OnDisconnected += on_disconnected;
        }

        private void on_disconnected()
        {
            // //向服务端发送断开通知
            m_channel?.Send(new EventPacket(new EventMsg(EventCode.ON_CLIENT_DISCONNECTING, m_channel.Id)));
            m_channel?.Dispose();
            m_channel = null;
        }

        private IntervalExecuteHelper interval_exe = new IntervalExecuteHelper(1);
        public override void DoUpdate()
        {
            base.DoUpdate();
            //每隔一段时间ping一次
            // interval_exe.Invoke(ping);
        }

        private void ping()
        {
            m_channel?.Send(new EventPacket(new EventMsg(EventCode.PING, BytesHelpper.long2bytes(System.DateTime.Now.ToBinary()), m_channel.Id)));
        }

        //响应获取从服务器发来的ChannelId
        protected void EVENT_GET_CHANNEL_ID(object param)
        {
            EventMsg msg = (EventMsg)param;
            m_channel.Id = msg.Id;
            Debug.Log($"CLIENT :: EVENT_GET_CHANNEL_ID --> Your client id is {msg.Id} to server");
            m_channel?.Send(new EventPacket(new EventMsg(EventCode.RCP_FROM_CLIENT, msg.Id)));
        }

        protected void EVENT_PING_RPC(object param)
        {
            EventMsg msg = (EventMsg)param;
            byte[] content = msg.Content;
            long databinary = BytesHelpper.bytes2long(content);
            System.DateTime date = System.DateTime.FromBinary(databinary);
            double milliseconds = (System.DateTime.Now - date).TotalMilliseconds;
            Debug.Log($"CLIENT :: clinet {m_channel.Id} ping is {milliseconds} ms ");
        }

        protected CommonStatusMgr common_status = MgrsContainer.GetMgr<CommonStatusMgr>();
        protected UIMgr ui = MgrsContainer.GetMgr<UIMgr>();
        protected void EVENT_CALL_JOIN_IN_REQUEST_METHOD(object param)
        {
            Debug.Log($"CLIENT :: EVENT_CALL_JOIN_IN_REQUEST_METHOD");
            m_channel?.Send(new EventPacket(new EventMsg(EventCode.JOIN_IN, (byte[])param, m_channel.Id)));
        }

        protected void EVENT_HAS_JOINED_ROOM_RPC(object param)
        {
            Debug.Log($"CLIENT :: EVENT_HAS_JOINED_ROOM_RPC");
            ui.LoadCanvasUI<AlertUIView>().SetText("HAS JOINED ROOM!");
        }

        protected void EVENT_JOIN_IN_RPC(object param)
        {
            // EventMsg msg = (EventMsg)param;
            // int id_room = unpack_room_info(msg);
            Debug.Log($"CLIENT :: EVENT_JOIN_IN_RPC");
            ui.LoadCanvasUI<RoomUIView>();
        }

        protected void EVENT_DONT_FIND_ROOM_RPC(object param)
        {
            Debug.Log($"CLIENT :: EVENT_DONT_FIND_ROOM_RPC");
            // Alert UI
            ui.LoadCanvasUI<AlertUIView>().SetText("DONT FIND ROOM");
        }

        protected void EVENT_CALL_CREATE_ROOM_REQUEST_METHOD(object param)
        {
            Debug.Log($"CLIENT :: EVENT_CALL_CREATE_ROOM_REQUEST_METHOD");
            m_channel?.Send(new EventPacket(new EventMsg(EventCode.CREATE_ROOM, (byte[])param, m_channel.Id)));
        }

        protected void EVENT_CREATE_ROOM_RPC(object param)
        {
            Debug.Log($"CLIENT :: EVENT_CREATE_ROOM_RPC");
            ui.LoadCanvasUI<RoomUIView>();
        }

        protected void EVENT_UPDATE_ROOM_RPC(object param)
        {
            EventMsg msg = (EventMsg)param;
            // 解msg
            int id_room = unpack_room_info(msg);
            Debug.Log($"CLIENT :: EVENT_UPDATE_ROOM_RPC --> ROOM_ID {id_room}");
        }

        protected void EVENT_CALL_EXIT_ROOM_METHOD(object param)
        {
            m_channel?.Send(new EventPacket(new EventMsg(EventCode.EXIT_ROOM, m_channel.Id)));
        }

        protected void EVENT_EXIT_ROOM_RPC(object param)
        {
            common_status.PlayerId = string.Empty;
            ui.UnloadCanvasUI<RoomUIView>();
            ui.LoadCanvasUI<GameStartUIView>();
        }

        protected void EVENT_CALL_GET_READY_METHOD(object param)
        {
            m_channel?.Send(new EventPacket(new EventMsg(EventCode.GET_READY, m_channel.Id)));
        }

        protected void EVENT_CALL_CANCEL_READY_METHOD(object param)
        {
            m_channel?.Send(new EventPacket(new EventMsg(EventCode.CANCEL_READY, m_channel.Id)));
        }

        protected void EVENT_CALL_PLAY_METHOD(object param)
        {
            m_channel?.Send(new EventPacket(new EventMsg(EventCode.PLAY, m_channel.Id)));
        }

        protected void EVENT_ROOM_IS_NOT_FULL_RPC(object param)
        {
            ui.LoadCanvasUI<AlertUIView>().SetText("ROOM ISN'T FULL");
        }

        protected void EVENT_ROOM_NOT_ALL_ARE_READY_RPC(object param)
        {
            ui.LoadCanvasUI<AlertUIView>().SetText("NOT ALL ARE READY");
        }

        protected void EVENT_PLAY_RPC(object param)
        {
            //退出RoomUIView
            ui.UnloadAll();
            //进入加载UIView
            ui.LoadCanvasUI<LoadingUIView>();
            //加载场景、角色、输入管理等服务
            EventMsg msg = (EventMsg)param;
            MgrsContainer.GetMgr<SceneMgr>().LoadNecessaryAssets(msg);
        }

        protected void EVENT_CALL_SYNC_LOAD_PROGRESS_METHOD(object param)
        {
            ByteBuf buf = new ByteBuf();
            buf.WriteLFloat(((float)param).ToLFloat());
            m_channel.Send(new EventPacket(new EventMsg(EventCode.SYNC_LOAD_PROGRESS, buf.Data, m_channel.Id)));
        }

        protected void EVENT_SYNC_LOAD_PROGRESS_RPC(object param)
        {
            EventMsg msg = (EventMsg)param;
            unpack_load_progresses(msg);
        }

        protected void EVENT_CALL_LOADING_COMPLETED_METHOD(object param)
        {
            m_channel.Send(new EventPacket(new EventMsg(EventCode.LOADING_COMPLETED, m_channel.Id)));
        }

        protected void EVENT_LOADING_COMPLETED_RPC(object param)
        {
            ui.UnloadCanvasUI<LoadingUIView>();
        }

        private int unpack_room_info(EventMsg msg)
        {
            ByteBuf buffer = new ByteBuf(msg.Content);
            int id_room = buffer.ReadInt();
            ui.LoadCanvasUI<RoomUIView>().SetRoomId(id_room);
            byte capicity = buffer.ReadByte();
            byte count_unit = buffer.ReadByte();
            ui.LoadCanvasUI<RoomUIView>().SetPlayers(count_unit, capicity);
            ui.LoadCanvasUI<RoomUIView>().InActiveAllUnits();
            for (byte i = 0; i < count_unit; i++)
            {
                string player_id = buffer.ReadString();
                bool isHost = buffer.ReadBool();
                bool isReady = buffer.ReadBool();
                if (player_id == common_status.PlayerId)
                {
                    ui.LoadCanvasUI<RoomUIView>().Button_play.SetActive(isHost);
                    ui.LoadCanvasUI<RoomUIView>().Button_ready.SetActive(!isHost);
                    if (isHost)
                    {
                        ui.LoadCanvasUI<RoomUIView>().Button_cancel.SetActive(false);
                        ui.LoadCanvasUI<RoomUIView>().Button_exit.GetComponent<UnityEngine.UI.Button>().interactable = true;
                    }
                    else
                    {
                        ui.LoadCanvasUI<RoomUIView>().Button_ready.SetActive(!isReady);
                        ui.LoadCanvasUI<RoomUIView>().Button_cancel.SetActive(isReady);
                        ui.LoadCanvasUI<RoomUIView>().Button_exit.GetComponent<UnityEngine.UI.Button>().interactable = !isReady;
                    }
                }
                ui.LoadCanvasUI<RoomUIView>().SetUnit(i, player_id, isHost, isReady);
            }
            return id_room;
        }
        private void unpack_load_progresses(EventMsg msg)
        {
            ByteBuf buffer = new ByteBuf(msg.Content);
            byte count_unit = buffer.ReadByte();
            ui.LoadCanvasUI<LoadingUIView>().InActiveAllUnits();
            LFloat total_progress = 0;
            for (byte i = 0; i < count_unit; i++)
            {
                string player_id = buffer.ReadString();
                LFloat load_progress = buffer.ReadLFloat();
                total_progress += load_progress;
                ui.LoadCanvasUI<LoadingUIView>().SetUnit(i, player_id, load_progress);
            }
            ui.LoadCanvasUI<LoadingUIView>().SetMainProgress(total_progress / count_unit);

        }
    }
}