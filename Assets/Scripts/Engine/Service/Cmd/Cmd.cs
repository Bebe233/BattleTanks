using BEBE.Engine.Interface;
using BEBE.Engine.Service.Net;
namespace BEBE.Engine.Service.Cmd
{
    public abstract class Cmd : ISerializable
    {
        //| frame | cmd1 | cmd2 | cmd3 | cmd4 | cmd5 | cmd6 |
        public virtual byte[] GetBytes()
        {
            ByteBuf buf = new ByteBuf();
            Serialize(ref buf);
            return buf.Data;
        }
        public virtual void PutBytes(byte[] bytes)
        {
            ByteBuf buffer =new ByteBuf(bytes);
            Deserialize(buffer);
        }

        public abstract void Serialize(ref ByteBuf buffer);
        public abstract void Deserialize(ByteBuf buffer);
    }
}