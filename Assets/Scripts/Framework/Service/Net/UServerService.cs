using BEBE.Engine.Service.Net;
using BEBE.Framework.Event;
using BEBE.Framework.Module;
using BEBE.Framework.Service.Net.Msg;
using BEBE.Engine.Logging;
using System;
using System.Net.Sockets;
using BEBE.Framework.Managers;

namespace BEBE.Framework.Service.Net
{
    public class UServerService : ServerService
    {
        public UServerService(string ip_address, int port) : base(ip_address, port)
        {
        }

        protected override void register_events()
        {
            Dispatchor.Register(this, Constant.EVENT_PREFIX);

            OnClientAccepted += on_client_accepted;
        }

        private void on_client_accepted(int channel_id, TcpClient acceptor)
        {
            UChannel channel = new UChannel(channel_id, acceptor);
            USession session = new USession(channel);
            m_sessions.AddOrUpdate(channel_id, session, (id, channel) => channel);
            //将id返回给client
            channel.Send(new EventPacket(new EventMsg(Event.EventCode.GET_CHANNEL_ID, channel.Id)));
        }

        protected void EVENT_RCP_FROM_CLIENT(object param)
        {
            EventMsg msg = (EventMsg)param;
            Debug.Log($"SERVER :: EVENT_RCP_FROM_CLIENT --> rcp from client {msg.Id} ");
        }

        protected void EVENT_ON_CLIENT_DISCONNECTING(object param)
        {
            EventMsg msg = (EventMsg)param;
            Debug.Log($"SERVER :: EVENT_ON_CLIENT_DISCONNECTING --> client {msg.Id} is disconnecting");
            if (m_sessions.TryRemove(msg.Id, out Session session))
            {
                session.Dispose();
                session = null;
            }
        }

        protected void EVENT_PING(object param)
        {
            EventMsg msg = (EventMsg)param;
            if (m_sessions.TryGetValue(msg.Id, out Session session))
            {
                session.Send(new EventPacket(new EventMsg(EventCode.PING_RPC, msg.Content, session.Id)));
            }
        }

        private void EVENT_JOIN_IN(object param)
        {
            EventMsg msg = (EventMsg)param;
            int channel_id = msg.Id;
            ByteBuf buf = new ByteBuf(msg.Content);
            String player_id = buf.ReadString();
            Debug.Log($"SERVER :: EVENT_JOIN_IN from client {channel_id} {player_id}");
            //find a room
            MgrsContainer.GetMgr<RoomMgr>().FindRoom((USession)m_sessions[channel_id], player_id);
        }

        private void EVENT_CREATE_ROOM(object param)
        {
            EventMsg msg = (EventMsg)param;
            int channel_id = msg.Id;
            ByteBuf buf = new ByteBuf(msg.Content);
            String player_id = buf.ReadString();
            Debug.Log($"SERVER :: EVENT_CREATE_ROOM {channel_id} {player_id}");
            MgrsContainer.GetMgr<RoomMgr>().CreateRoom((USession)m_sessions[channel_id], player_id);
        }

        private void EVENT_EXIT_ROOM(object param)
        {
            EventMsg msg = (EventMsg)param;
            int channel_id = msg.Id;
            MgrsContainer.GetMgr<RoomMgr>().ExitRoom((USession)m_sessions[channel_id]);
        }

        private void EVENT_GET_READY(object param)
        {
            EventMsg msg = (EventMsg)param;
            int channel_id = msg.Id;
            MgrsContainer.GetMgr<RoomMgr>().GetReady((USession)m_sessions[channel_id]);
        }

        private void EVENT_CANCEL_READY(object param)
        {
            EventMsg msg = (EventMsg)param;
            int channel_id = msg.Id;
            MgrsContainer.GetMgr<RoomMgr>().CancelReady((USession)m_sessions[channel_id]);
        }

        private void EVENT_PLAY(object param)
        {
            EventMsg msg = (EventMsg)param;
            int channel_id = msg.Id;
            MgrsContainer.GetMgr<RoomMgr>().Play((USession)m_sessions[channel_id]);
        }
    }
}