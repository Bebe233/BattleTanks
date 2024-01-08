
using System;

namespace BEBE.Engine.Service.Net
{
    public class Session : IDisposable
    {
        public int Id => channel.Id;
        protected Channel channel;

        public Session(Channel channel)
        {
            this.channel = channel;
        }

        public void Dispose()
        {
            channel.Dispose();
        }

        public void RecieveMsg()
        {
            channel.RecieveMsg();
        }

        public void Send(Packet packet)
        {
            channel.Send(packet);
        }
    }
}