using System.Collections.Generic;
using BEBE.Engine.Math;
using BEBE.Engine.Service.Cmd;
using BEBE.Engine.Service.Net;
using System;

namespace BEBE.Framework.Component
{
    public abstract class BInput : Cmd, IEquatable<BInput>
    {
        public LFloat x, y; //axis raw value 摇杆/方向

        public override byte[] GetBytes()
        {
            ByteBuf buf = new ByteBuf();
            Serialize(ref buf);
            return buf.Data;
        }

        public override void PutBytes(byte[] bytes)
        {
            ByteBuf buf = new ByteBuf(bytes);
            Deserialize(buf);
        }

        public override void Deserialize(ByteBuf buffer)
        {
            base.Deserialize(buffer);
            x = buffer.ReadLFloat();
            y = buffer.ReadLFloat();
        }

        public override void Serialize(ref ByteBuf buffer)
        {
            base.Serialize(ref buffer);
            buffer.WriteLFloat(x);
            buffer.WriteLFloat(y);
        }

        public bool Equals(BInput other)
        {
            if (this.tick != other.tick) return false;
            if (this.x != other.x) return false;
            if (this.y != other.y) return false;
            return true;
        }

    }

    public class PlayerInput : BInput, IEquatable<PlayerInput>
    {
        public byte actorId;
        public bool executed = false; //标记是否被entity执行

        public override void Serialize(ref ByteBuf buffer)
        {
            base.Serialize(ref buffer);
            buffer.WriteByte(actorId);
        }

        public override void Deserialize(ByteBuf buffer)
        {
            base.Deserialize(buffer);
            actorId = buffer.ReadByte();
        }

        public override void PutBytes(byte[] bytes)
        {
            ByteBuf buf = new ByteBuf(bytes);
            Deserialize(buf);
        }

        public override byte[] GetBytes()
        {
            ByteBuf buf = new ByteBuf();
            Serialize(ref buf);
            return buf.Data;
        }

        public bool Equals(PlayerInput other)
        {
            if (this.actorId != other.actorId) return false;
            if (this.tick != other.tick) return false;
            if (this.x != other.x) return false;
            if (this.y != other.y) return false;
            return true;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"\n<---------Tick {tick}---------->");
            sb.Append(" actorId ");
            sb.Append(actorId);
            sb.Append(" x ");
            sb.Append(x);
            sb.Append(" y ");
            sb.Append(y);
            sb.AppendLine("\n<------------------->");
            return sb.ToString();
        }
    }

    public class PlayerInputs : Cmd
    {
        Dictionary<int, PlayerInput> playerInputs;
        public Dictionary<int, PlayerInput> Inputs => playerInputs;

        public PlayerInputs()
        {
            playerInputs = new Dictionary<int, PlayerInput>();
        }

        public bool put(PlayerInput input)
        {
            return put(input.tick, input);
        }

        public bool put(int tick, PlayerInput input)
        {
            if (playerInputs.TryAdd(tick, input))
                return true;
            else
                return false;
        }

        public bool get(int tick, out PlayerInput input)
        {
            if (playerInputs.TryGetValue(tick, out input))
                return true;
            else
                return false;
        }

        public bool delete(int tick)
        {
            return playerInputs.Remove(tick);
        }

        public override void Deserialize(ByteBuf buffer)
        {
            base.Deserialize(buffer);
            int num = buffer.ReadInt();
            for (int i = 0; i < num; i++)
            {
                PlayerInput input = new PlayerInput();
                input.Deserialize(buffer);
                put(input);
            }
        }

        public override void Serialize(ref ByteBuf buffer)
        {
            base.Serialize(ref buffer);
            buffer.WriteInt(playerInputs.Count);
            foreach (var pInput in playerInputs.Values)
            {
                pInput.Serialize(ref buffer);
            }
        }

        public override byte[] GetBytes()
        {
            ByteBuf buf = new ByteBuf();
            Serialize(ref buf);
            // BEBE.Engine.Logging.Debug.Log($"buf {buf.Length}");
            return buf.Data;
        }

        public override void PutBytes(byte[] bytes)
        {
            ByteBuf buf = new ByteBuf(bytes);
            Deserialize(buf);
            // BEBE.Engine.Logging.Debug.Log($"{AllInputs.Count}");
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"\n<---------Tick {tick}---------->");
            foreach (var input in playerInputs.Values)
            {
                sb.Append(" tick ");
                sb.Append(input.tick);
                sb.Append(" x ");
                sb.Append(input.x);
                sb.Append(" y ");
                sb.Append(input.y);
                sb.AppendLine(" |");
            }
            sb.AppendLine("<------------------->");
            return sb.ToString();
        }
    }

    public class PlayersInputs : Cmd
    {
        // Dictionary<byte, PlayerInputs> playersInputs = new Dictionary<byte, PlayerInputs>();

        // public bool put(PlayerInputs inputs)
        // {
        //     return put(input.tick, input);
        // }

        // public bool put(byte actor, PlayerInput input)
        // {
        //     if (playerInputs.TryAdd(tick, input))
        //         return true;
        //     else
        //         return false;
        // }

        public override void PutBytes(byte[] bytes)
        {

        }

        public override byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}