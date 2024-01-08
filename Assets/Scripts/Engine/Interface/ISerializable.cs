using BEBE.Engine.Service.Net;
namespace BEBE.Engine.Interface
{
    public interface ISerializable
    {
        void Serialize(ref ByteBuf buffer);
        void Deserialize(ByteBuf buffer);
      
    }
}