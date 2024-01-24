// 一帧的所有输入
using System;
using System.Collections.Generic;
using BEBE.Engine.Service.Cmd;
using BEBE.Engine.Service.Net;
using BEBE.Framework.Managers;

namespace BEBE.Game.Inputs
{
    public class TickInputs : Cmd, IEquatable<TickInputs>
    {
        // tick | count | input ... |
        public int Tick => tick;
        public void SetTick(int tick)
        {
            this.tick = tick;
        }
        private int tick = -1;
        public PlayerInput[] Inputs => inputs;
        private PlayerInput[] inputs;

        public TickInputs()
        {
        }

        public TickInputs(int tick, byte capicity)
        {
            this.tick = tick;
            inputs = new PlayerInput[capicity];
        }


        public void Put(PlayerInput input)
        {
            inputs[input.actorId] = input;
        }

        public PlayerInput Get(byte actorId)
        {
            return inputs[actorId];
        }

        public TickInputs Clone(int tick)
        {
            TickInputs res = new TickInputs();
            res.PutBytes(GetBytes());
            res.SetTick(tick);
            return res;
        }

        public override void Serialize(ref ByteBuf buffer)
        {
            buffer.WriteInt(tick);
            buffer.WriteByte((byte)inputs.Length);
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i].Serialize(ref buffer);
            }
        }

        public override void Deserialize(ByteBuf buffer)
        {
            tick = buffer.ReadInt();
            byte length = buffer.ReadByte();
            inputs = new PlayerInput[length];
            for (int i = 0; i < length; i++)
            {
                PlayerInput input = new PlayerInput();
                input.Deserialize(buffer);
                inputs[input.actorId] = input;
            }
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"\n<--------Tick {tick}----------->");
            for (int i = 0; i < inputs.Length; i++)
            {
                var actor = inputs[i];
                sb.Append(" actorId -->");
                sb.Append(actor.actorId);
                sb.Append(" x -->");
                sb.Append(actor.x);
                sb.Append(" y -->");
                sb.Append(actor.y);
            }
            sb.AppendLine("\n<------------------->");
            return sb.ToString();
        }

        public bool Equals(TickInputs other)
        {
            int length = Inputs.Length;
            int length_other = other.Inputs.Length;
            if (length != length_other) return false;
            for (int i = 0; i < length; i++)
            {
                if (!Inputs[i].Equals(other.Inputs[i])) return false;
            }
            return true;
        }
    }
}