using System.Collections.Concurrent;
using BEBE.Engine.Service.Net;

namespace BEBE.Framework.Managers
{
    public class RoomMgr : IMgr
    {
        ConcurrentDictionary<int, Room> rooms = new ConcurrentDictionary<int, Room>();
        

        protected void EVENT_ON_JOIN_REQUEST_RECV(object param)
        {
            EventMsg msg = (EventMsg)param;
            BEBE.Engine.Logging.Debug.Log($"EVENT_ON_JOIN_REQUEST_RECV --> client {msg.Id}");
            //判断是否有Room

        }
    }
}