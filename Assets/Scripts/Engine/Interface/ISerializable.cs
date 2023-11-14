using BEBE.Framework.Service.Net;

namespace BEBE.Framework.Interface
{
    public interface ISerializable
    {
        void Serialize();
        void Deserialize();
    }
}