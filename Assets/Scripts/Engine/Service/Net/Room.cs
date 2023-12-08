using System.Collections.Generic;
namespace BEBE.Engine.Service.Net
{
    public class Room
    {
        public const int capicity = 10;

        private int id;
        private Dictionary<int, Channel> channels;

        public bool IsFull => channels.Count >= 10;

        public Room(int id)
        {
            this.id = id;
            channels = new Dictionary<int, Channel>(capicity);
        }

        public void Join(Channel channel)
        {

        }


    }
}