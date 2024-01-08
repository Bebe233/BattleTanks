using System.IO;
using BEBE.Engine.Service.Net;
using BEBE.Framework.Service.Net.Msg;

namespace BEBE.Framework.Service.Net
{
    public class EventPacket : Packet
    {
        public EventPacket()
        {

        }

        public EventPacket(BinaryReader reader) : base(reader)
        {
        }

        public EventPacket(EventMsg event_msg)
        {
            event_msg.Serialize(ref buffer);
        }

        public EventMsg ParseEventMsg()
        {
            var res = new EventMsg();
            res.Deserialize(buffer);
            return res;
        }

        public void Duplicate(Packet src)
        {
            this.buffer = src.Buffer;
            this.msgType = src.MsgType;
        }

        public static EventPacket Wrap(Packet packet)
        {
            EventPacket p = new EventPacket();
            p.Duplicate(packet);
            return p;
        }
    }
}