using System.IO;
using BEBE.Engine.Logging;

namespace BEBE.Engine.Service.Net
{
    public class Packet
    {
        // | length (4 byte) | msgType (1 byte) | buffer |
        public int Length => buffer.Capacity;
        protected MsgType msgType;
        public MsgType MsgType => msgType;
        protected ByteBuf buffer = new ByteBuf();
        public ByteBuf Buffer => buffer;

        public Packet(){

        }

        public Packet(BinaryReader reader)
        {
            buffer.MarkReaderIndex();
            buffer.WriteBytes(reader);
            msgType = (MsgType)buffer.ReadByte();
            buffer.ResetReaderIndex();
        }
      
    }
}