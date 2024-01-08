using BEBE.Engine.Service.Net;
using BEBE.Engine.Math;
namespace BEBE.Framework.Service.Net
{
    public class USession : Session
    {
        public bool IsHost { get; set; }
        public bool HasJoinedRoom { get; set; }
        public bool IsReady { get; set; }
        public int RoomId { get; set; }
        public string PlayerId { get; set; }
        public LFloat LoadingProgress { get; set; }
        public USession(Channel channel) : base(channel)
        {
        }
    }
}