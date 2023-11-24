using BEBE.Engine.Service.Net;
namespace BEBE.Engine.Interface
{
    public interface ISerializable
    {
        void Serialize(ref ByteBuf buffer, bool resetWriteIndex = true);
        void Deserialize(ByteBuf buffer, bool resetReadIndex = true);
    }
}