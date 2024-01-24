using System.Collections.Generic;
using BEBE.Engine.Service.Cmd;
using BEBE.Engine.Service.Net;
using System.Collections.Concurrent;
namespace BEBE.Game.Inputs
{
    public class TickInputsCache : Cmd
    {
        // | count | TickIputs ... |
        public int Count => cache.Count;
        private ConcurrentQueue<TickInputs> cache = new ConcurrentQueue<TickInputs>();
        public override void Serialize(ref ByteBuf buffer)
        {
            buffer.WriteInt(cache.Count);
            while (TryGet(out TickInputs input))
            {
                input.Serialize(ref buffer);
            }
        }

        public override void Deserialize(ByteBuf buffer)
        {
            int count = buffer.ReadInt();
            cache = new ConcurrentQueue<TickInputs>();
            for (int i = 0; i < count; i++)
            {
                TickInputs inputs = new TickInputs();
                inputs.Deserialize(buffer);
                cache.Enqueue(inputs);
            }
        }


        public void Put(TickInputs inputs)
        {
            cache.Enqueue(inputs);
        }


        public bool TryGet(out TickInputs inputs)
        {
            return cache.TryDequeue(out inputs);
        }

        public bool TryPeek(out TickInputs inputs)
        {
            return cache.TryPeek(out inputs);
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            while (TryGet(out TickInputs input))
            {
                sb.AppendLine(input.ToString());
            }
            return sb.ToString();
        }
    }
}