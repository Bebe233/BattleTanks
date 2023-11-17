using BEBE.Engine.Interface;
using BEBE.Engine.Service.Net;

namespace BEBE.Engine.Service.Cmd
{
    public abstract class Cmd : ISerializable
    {
        //| frame | cmd1 | cmd2 | cmd3 | cmd4 | cmd5 | cmd6 |
        public virtual void Deserialize(ByteBuf buffer)
        {

        }

        public virtual void Serialize(ref ByteBuf buffer)
        {

        }

        public abstract byte[] GetBytes();

        public abstract void DecodeBytes(byte[] bytes);
    }
}