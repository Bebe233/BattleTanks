using System.Collections.Generic;
using BEBE.Engine.Logging;
using BEBE.Engine.Interface;
using BEBE.Engine.Service.Net;
using System;
using BEBE.Framework.Service.Net.Msg;
using BEBE.Framework.Event;
using System.Linq;

namespace BEBE.Framework.Service.Net
{
    //收集
    public class Room
    {
        protected byte capicity = 10;
        private int id;
        public int Id => id;
        private Dictionary<string, USession> sessions = new Dictionary<string, USession>();

        public bool IsFull => sessions.Count >= capicity;
        public bool IsEmpty => sessions.Count == 0;
        public bool AreAllReady => !sessions.Any(pair => pair.Value.IsReady == false);
        public bool AreAllLoadingCompleted => !sessions.Any(pair => pair.Value.IsLoadingCompleted == false);
        public Room()
        {

        }

        public Room(int id, byte capicity = 10)
        {
            this.id = id;
            this.capicity = capicity;
        }

        public void Broadcast(EventPacket eventPacket)
        {
            foreach (var session in sessions.Values)
            {
                session.Send(eventPacket);
            }
        }

        public virtual void Join(USession session, string player_id)
        {
            if (IsFull) return;
            sessions.Add(player_id, session);
            session.HasJoinedRoom = true;
            session.RoomId = id;
            session.PlayerId = player_id;
            Debug.Log($"Player {player_id} joined room {id}");

        }


        public void Exit(string player_id)
        {
            if (sessions.ContainsKey(player_id))
            {
                USession s = sessions[player_id];
                s.HasJoinedRoom = false;
                s.RoomId = 0;
                //设置一个新的房主
                if (s.IsHost)
                {
                    s.IsHost = false;
                    s.IsReady = false;
                    foreach (var session in sessions.Values)
                    {
                        if (session == s) continue;
                        session.IsHost = true;
                        break;
                    }
                }
                s.Send(new EventPacket(new EventMsg(Event.EventCode.EXIT_ROOM_RPC)));
                //RPC Client exit 
                sessions.Remove(player_id);
                Broadcast(new EventPacket(new EventMsg(EventCode.UPDATE_ROOM_RPC, GetRoomInfo(), -1)));
            }
        }

        public byte[] GetRoomInfo()
        {
            ByteBuf buffer = new ByteBuf();
            buffer.WriteInt(id);
            buffer.WriteByte(capicity);
            buffer.WriteByte((byte)sessions.Count);
            foreach (var session in sessions)
            {
                buffer.WriteString(session.Key);
                buffer.WriteBool(session.Value.IsHost);
                buffer.WriteBool(session.Value.IsReady);
            }
            return buffer.Data;
        }

        public byte[] GetLoadingProgress()
        {
            ByteBuf buffer = new ByteBuf();
            buffer.WriteByte((byte)sessions.Count);
            foreach (var session in sessions)
            {
                buffer.WriteString(session.Key);
                buffer.WriteLFloat(session.Value.LoadingProgress);
            }
            return buffer.Data;
        }

    }
}