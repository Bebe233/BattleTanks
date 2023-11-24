using BEBE.Engine.Interface;
using BEBE.Engine.Service.Net;
namespace BEBE.Engine.Service.Cmd
{
    public abstract class Cmd : ISerializable
    {
        public int tick;
        //| frame | cmd1 | cmd2 | cmd3 | cmd4 | cmd5 | cmd6 |
        public virtual void Deserialize(ByteBuf buffer, bool resetReadIndex = true)
        {
            if (resetReadIndex)
                buffer.ResetReadIndex();
            tick = buffer.ReadInt();
        }

        public virtual void Serialize(ref ByteBuf buffer, bool resetWriteIndex = false)
        {
            if (resetWriteIndex)
                buffer.ResetWriteIndex();
            buffer.WriteInt(tick);
        }

        public abstract byte[] GetBytes();

        public abstract void DecodeBytes(byte[] bytes);

    }
}