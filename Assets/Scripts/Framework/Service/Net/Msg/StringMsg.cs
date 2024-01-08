using BEBE.Engine.Interface;
using BEBE.Engine.Service.Net;
using BEBE.Framework.Event;

namespace BEBE.Framework.Service.Net.Msg
{
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
            len_payload = content.Length + sizeof(byte) + sizeof(int);
        }

        public void Serialize(ref ByteBuf buffer)
        {
            buffer.WriteInt(len_payload);
            buffer.WriteByte(flag);
            buffer.WriteInt(id);
            buffer.WriteString(content);
        }

        public void Deserialize(ByteBuf buffer)
        {
            len_payload = buffer.ReadInt();
            flag = buffer.ReadByte();
            id = buffer.ReadInt();
            content = System.Text.Encoding.UTF8.GetString(buffer.ReadBytes());
        }
    }
}