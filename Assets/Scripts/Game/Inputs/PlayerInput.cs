using System;
using BEBE.Engine.Service.Cmd;
using BEBE.Engine.Service.Net;
using BEBE.Framework.Component;

namespace BEBE.Game.Inputs
{
    public class PlayerInput : BInput, IEquatable<PlayerInput>
    {
        public PlayerInput()
        {

        }
        public PlayerInput(byte actorId)
        {
            this.actorId = actorId;
        }

        public PlayerInput RollbackInput()
        {
            PlayerInput res = new PlayerInput(actorId);
            res.executed = false;
            res.x = -x;
            res.y = -y;
            return res;
        }

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

        public bool Equals(PlayerInput other)
        {
            if (this.actorId != other.actorId) return false;
            if (this.x != other.x) return false;
            if (this.y != other.y) return false;
            return true;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"\n<------------------->");
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

    public class PlayerInputData : Cmd
    {
        // tick | input |
        public PlayerInput PlayerInput => player_input;
        private PlayerInput player_input;
        public int Tick => tick;
        private int tick = -1;

        public PlayerInputData()
        {

        }
        public PlayerInputData(int tick, PlayerInput input)
        {
            this.tick = tick;
            this.player_input = input;
        }

        public override void Serialize(ref ByteBuf buffer)
        {
            buffer.WriteInt(tick);
            player_input.Serialize(ref buffer);
        }

        public override void Deserialize(ByteBuf buffer)
        {
            tick = buffer.ReadInt();
            player_input = new PlayerInput();
            player_input.Deserialize(buffer);
        }
    }
}