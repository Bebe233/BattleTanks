using System;
using BEBE.Engine.Event;
using BEBE.Engine.Interface;
namespace BEBE.Engine.Service.Net
{
    public abstract class BaseMsg
    {
        protected byte flag;
        public MsgType Flag => (MsgType)flag;
        protected int id = -1; //客户端id 省缺值为-1
        public int Id => id;
        protected int length_body; //消息长度
        public int Length_body => Length_body;
    }

    public class StringMsg : BaseMsg, ISerializable
    {
        protected string content;
        public string Content => content;
        public StringMsg()
        {
        }
        public StringMsg(string content, int id = -1)
        {
            flag = ((byte)MsgType.String);
            this.content = content;
            this.id = id;
            length_body = content.Length + sizeof(byte) + sizeof(int);
        }

        public void Serialize(ref ByteBuf buffer)
        {
            buffer.ResetWriteIndex();
            buffer.WriteInt(length_body);
            buffer.WriteByte(flag);
            buffer.WriteInt(id);
            buffer.WriteString(content);
        }

        public void Deserialize(ByteBuf buffer)
        {
            buffer.ResetReadIndex();
            length_body = buffer.ReadInt();
            flag = buffer.ReadByte();
            id = buffer.ReadInt();
            content = System.Text.Encoding.UTF8.GetString(buffer.ReadBytes());
        }
    }

    public class EventMsg : BaseMsg, ISerializable
    {
        protected byte eventCode;
        public EventCode EventCode => (EventCode)eventCode;
        protected byte[] content;
        public byte[] Content => content;
        public EventMsg()
        {

        }

        public EventMsg(EventCode eCode, int id) : this(eCode, null, id)
        {

        }

        public EventMsg(EventCode eCode, byte[] content, int id = -1)
        {
            flag = ((byte)MsgType.EventCode);
            this.eventCode = ((byte)eCode);
            this.id = id;
            length_body = 6;
            if (content != null)
            {
                this.content = content;
                length_body += content.Length;
            }
        }

        public void Serialize(ref ByteBuf buffer)
        {
            buffer.ResetWriteIndex();
            buffer.WriteInt(length_body);
            buffer.WriteByte(flag);
            buffer.WriteInt(id);
            buffer.WriteByte(eventCode);
            buffer.WriteBytes(content);
        }

        public void Deserialize(ByteBuf buffer)
        {
            buffer.ResetReadIndex();
            length_body = buffer.ReadInt();
            flag = buffer.ReadByte();
            id = buffer.ReadInt();
            eventCode = buffer.ReadByte();
            content = buffer.ReadBytes();
        }

    }

}