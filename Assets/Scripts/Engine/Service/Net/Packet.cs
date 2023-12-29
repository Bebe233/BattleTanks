using System.IO;
using BEBE.Engine.Event;
using BEBE.Engine.Managers;
using BEBE.Engine.Service.Net;

namespace BEBE.Engine.Service.Net
{
    public class Packet
    {
        // | length (4 byte) | msgType (1 byte) | buffer |
        public int Length => buffer.Length;
        protected MsgType msgType;
        public MsgType MsgType => msgType;
        protected ByteBuf buffer = new ByteBuf();
        public ByteBuf Buffer => buffer;
        public Packet(BinaryReader reader)
        {
            buffer.WriteBytes(reader);
            msgType = (MsgType)buffer.ReadByte();
        }

        public Packet(StringMsg str_msg)
        {
            str_msg.Serialize(ref buffer);
        }

        public Packet(EventMsg event_msg)
        {
            event_msg.Serialize(ref buffer);
        }

        public StringMsg ParseStringMsg()
        {
            var res = new StringMsg();
            res.Deserialize(buffer);
            return res;
        }

        public EventMsg ParseEventMsg()
        {
            var res = new EventMsg();
            res.Deserialize(buffer);
            return res;
        }
    }
}