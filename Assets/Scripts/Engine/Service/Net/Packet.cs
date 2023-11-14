using System.IO;
using BEBE.Framework.Event;
using BEBE.Framework.Managers;

namespace BEBE.Framework.Service.Net
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

        public Packet(string msg)
        {
            int length_body = msg.Length + 1;
            buffer.WriteInt(length_body);
            msgType = BEBE.Framework.Service.Net.MsgType.String;
            buffer.WriteByte(((byte)msgType));
            buffer.WriteString(msg);
        }

        public Packet(Event.EventCode eventCode, byte param)
        {
            int length_body = 4; // | msgType | eventCode | paramType | param |
            buffer.WriteInt(length_body);
            msgType = BEBE.Framework.Service.Net.MsgType.EventCode;
            buffer.WriteByte(((byte)msgType));
            buffer.WriteByte(((byte)eventCode));
            buffer.WriteByte(((byte)ParamType.BYTE));
            buffer.WriteByte(param);
        }

        public string DecodeString()
        {
            return System.Text.Encoding.UTF8.GetString(Buffer.ReadBytes());
        }

        public void DecodeEventCode(NetService sender)
        {
            //EventCode
            EventCode eCode = (EventCode)Buffer.ReadByte();
            ParamType pType = (ParamType)Buffer.ReadByte();

            if (Buffer.Length > 7) //有参
            {
                switch (pType)
                {
                    case ParamType.BYTE:
                        byte param = Buffer.ReadByte();
                        Dispatchor.Dispatch(sender, eCode, param);
                        break;
                }
            }
            else
            {
                Dispatchor.Dispatch(sender, eCode, null);
            }
        }
    }

}