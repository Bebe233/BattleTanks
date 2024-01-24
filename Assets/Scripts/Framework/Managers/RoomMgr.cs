using System;
using System.Linq;
using System.Collections.Concurrent;
using BEBE.Engine.Service.Net;
using BEBE.Engine.Service.Net.Utils;
using BEBE.Engine.Logging;
using BEBE.Framework.Service.Net.Msg;
using BEBE.Framework.Event;
using BEBE.Framework.Service.Net;
using BEBE.Framework.Component;
using BEBE.Framework.Module;

namespace BEBE.Framework.Managers
{
    //房间管理类
    //响应客户端的请求，并分配房间
    public class RoomMgr : IMgr
    {
        private ConcurrentDictionary<int, Room> id2room = new ConcurrentDictionary<int, Room>();
        private IdGenerator id_gen = new IdGenerator(100000);

        internal void CreateRoom(USession session, string player_id)
        {
            //判断是否已经在房间中
            if (session.HasJoinedRoom)
            {
                session.Send(new EventPacket(new EventMsg(EventCode.HAS_JOINED_ROOM_RPC)));
                return;
            }
            //随机分配一个room的id
            int id_room = id_gen.Get();
            Room room = new Room(id_room, Constant.ROOM_CAPICITY);
            id2room[id_room] = room;
            session.IsHost = true;
            session.IsReady = true;
            room.Join(session, player_id);
            session.Send(new EventPacket(new EventMsg(EventCode.CREATE_ROOM_RPC)));
            //broadcast 刷新room
            room.Broadcast(new EventPacket(new EventMsg(EventCode.UPDATE_ROOM_RPC, room.GetRoomInfo(), -1)));
        }

        internal void ExitRoom(USession uSession)
        {
            if (!uSession.HasJoinedRoom) return;
            if (!uSession.IsHost && uSession.IsReady) return;
            //get room
            if (id2room.TryGetValue(uSession.RoomId, out Room room))
            {
                room.Exit(uSession.PlayerId);
                //检测room 是否还有玩家，没有就删除room
                if (room.IsEmpty)
                {
                    id2room.TryRemove(room.Id, out Room value);
                }
            }
        }

        internal void FindRoom(USession session, string player_id)
        {
            try
            {
                //判断是否已经在房间中
                if (session.HasJoinedRoom)
                {
                    session.Send(new EventPacket(new EventMsg(EventCode.HAS_JOINED_ROOM_RPC)));
                    return;
                }
                var room_pair = id2room.Where(room => !room.Value.IsFull).First();
                room_pair.Value.Join(session, player_id);
                session.Send(new EventPacket(new EventMsg(EventCode.JOIN_IN_RPC)));
                room_pair.Value.Broadcast(new EventPacket(new EventMsg(EventCode.UPDATE_ROOM_RPC, room_pair.Value.GetRoomInfo(), -1)));
            }
            catch (Exception)
            {
                // Debug.LogException(e);
                //回应客户端，没有找到房间
                session.Send(new EventPacket(new EventMsg(EventCode.DONT_FIND_ROOM_RPC)));
            }
        }

        internal void GetReady(USession uSession)
        {
            if (!uSession.IsReady)
            {
                uSession.IsReady = true;
                //broadcast 刷新room
                if (id2room.TryGetValue(uSession.RoomId, out Room room))
                    room.Broadcast(new EventPacket(new EventMsg(EventCode.UPDATE_ROOM_RPC, room.GetRoomInfo(), -1)));
            }
        }

        internal void CancelReady(USession uSession)
        {
            if (uSession.IsReady)
            {
                uSession.IsReady = false;
                //broadcast 刷新room
                if (id2room.TryGetValue(uSession.RoomId, out Room room))
                    room.Broadcast(new EventPacket(new EventMsg(EventCode.UPDATE_ROOM_RPC, room.GetRoomInfo(), -1)));
            }
        }

        internal void Play(USession uSession)
        {
            if (id2room.TryGetValue(uSession.RoomId, out Room room))
            {
                if (room.IsFull)
                {
                    //确认所有人都准备好了
                    if (room.AreAllReady)
                    {
                        //开始游戏
                        room.Broadcast(new EventPacket(new EventMsg(EventCode.PLAY_RPC, room.GetRoomInfo(), -1)));
                    }
                    else
                    {
                        uSession.Send(new EventPacket(new EventMsg(EventCode.ROOM_NOT_ALL_ARE_READY_RPC)));
                    }

                }
                else
                {
                    uSession.Send(new EventPacket(new EventMsg(EventCode.ROOM_IS_NOT_FULL_RPC)));
                }
            }
        }

        internal void SyncLoadProgress(USession uSession, Engine.Math.LFloat progress)
        {
            uSession.LoadingProgress = progress;
            if (id2room.TryGetValue(uSession.RoomId, out Room room))
            {
                room.Broadcast(new EventPacket(new EventMsg(EventCode.SYNC_LOAD_PROGRESS_RPC, room.GetLoadingProgress(), -1)));
            }
        }

        internal void LoadingCompleted(USession uSession)
        {
            uSession.IsLoadingCompleted = true;
            if (id2room.TryGetValue(uSession.RoomId, out Room room))
            {
                if (room.AreAllLoadingCompleted)
                {
                    room.Broadcast(new EventPacket(new EventMsg(EventCode.LOADING_COMPLETED_RPC)));
                }
            }
        }

        internal void Broadcast(USession uSession, EventMsg msg)
        {
            if (id2room.TryGetValue(uSession.RoomId, out Room room))
            {
                room.Broadcast(new EventPacket(msg));
            }
        }
    }
}