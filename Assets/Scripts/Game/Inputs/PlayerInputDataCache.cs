using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BEBE.Engine.Service.Cmd;
using BEBE.Engine.Service.Net;

namespace BEBE.Game.Inputs
{
    public class PlayerInputDataCache : Cmd
    {
        // | count | inputData ... |
        private ConcurrentQueue<PlayerInputData> cache = new ConcurrentQueue<PlayerInputData>();
        public override void Serialize(ref ByteBuf buffer)
        {
            buffer.WriteInt(cache.Count);
            while (TryGet(out PlayerInputData input))
            {
                input.Serialize(ref buffer);
            }
        }

        public override void Deserialize(ByteBuf buffer)
        {
            int count = buffer.ReadInt();
            cache = new ConcurrentQueue<PlayerInputData>();
            for (int i = 0; i < count; i++)
            {
                PlayerInputData input = new PlayerInputData();
                input.Deserialize(buffer);
                cache.Enqueue(input);
            }
        }

        public void Put(PlayerInputData input)
        {
            cache.Enqueue(input);
        }


        public bool TryGet(out PlayerInputData input)
        {
            return cache.TryDequeue(out input);
        }
    }
}