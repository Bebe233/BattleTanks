using System.IO;
using BEBE.Engine.Service.Net;
using BEBE.Framework.Service.Net.Msg;

namespace BEBE.Framework.Service.Net
{
    public class StringPacket : Packet
    {
        public StringPacket()
        {

        }

        public StringPacket(BinaryReader reader) : base(reader)
        {
        }

        public StringPacket(StringMsg str_msg)
        {
            str_msg.Serialize(ref buffer);
        }

        public StringMsg ParseStringMsg()
        {
            var res = new StringMsg();
            res.Deserialize(buffer);
            return res;
        }

        public void Duplicate(Packet src)
        {
            this.buffer = src.Buffer;
            this.msgType = src.MsgType;
        }

        public static StringPacket Wrap(Packet packet)
        {
            StringPacket p = new StringPacket();
            p.Duplicate(packet);
            return p;
        }
    }
}