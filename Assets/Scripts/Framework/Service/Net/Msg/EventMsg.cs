using BEBE.Engine.Interface;
using BEBE.Engine.Service.Net;
using BEBE.Framework.Event;

namespace BEBE.Framework.Service.Net.Msg
{
    public class EventMsg : BaseMsg, ISerializable
    {
        protected byte eventCode;
        public EventCode EventCode => (EventCode)eventCode;
        protected byte[] content;
        public byte[] Content => content;
        public EventMsg()
        {

        }

        public EventMsg(EventCode eCode) : this(eCode, null, -1)
        {

        }

        public EventMsg(EventCode eCode, int id) : this(eCode, null, id)
        {

        }

        public EventMsg(EventCode eCode, byte[] content, int id = -1)
        {
            this.flag = ((byte)MsgType.EventCode);
            this.eventCode = ((byte)eCode);
            this.id = id;
            this.len_payload = 6;
            if (content != null)
            {
                this.content = content;
                this.len_payload += content.Length;
            }
        }

        public void Serialize(ref ByteBuf buffer)
        {
            buffer.WriteInt(len_payload);
            buffer.WriteByte(flag);
            buffer.WriteInt(id);
            buffer.WriteByte(eventCode);
            buffer.WriteBytes(content);
        }

        public void Deserialize(ByteBuf buffer)
        {
            len_payload = buffer.ReadInt();
            flag = buffer.ReadByte();
            id = buffer.ReadInt();
            eventCode = buffer.ReadByte();
            content = buffer.ReadBytes();
        }

    }
}