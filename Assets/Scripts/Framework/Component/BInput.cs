using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BEBE.Engine.Math;
using BEBE.Engine.Service.Cmd;
using BEBE.Engine.Service.Net;

namespace BEBE.Framework.Component
{
    public abstract class BInput : Cmd
    {
        public LFloat x, y; //axis raw value 摇杆/方向

        public override byte[] GetBytes()
        {
            ByteBuf buf = new ByteBuf();
            Serialize(ref buf);
            return buf.Data;
        }

        public override void DecodeBytes(byte[] bytes)
        {
            ByteBuf buf = new ByteBuf(bytes);
            Deserialize(buf);
        }

        public override void Deserialize(ByteBuf buffer)
        {
            buffer.ResetReadIndex();
            x = buffer.ReadLFloat();
            y = buffer.ReadLFloat();
        }

        public override void Serialize(ref ByteBuf buffer)
        {
            buffer.ResetWriteIndex();
            buffer.WriteLFloat(x);
            buffer.WriteLFloat(y);
        }
    }

    public class PlayerInput : BInput
    {

    }

    public class PlayerInputs : Cmd
    {
        public int Capicity { get; private set; }

        Dictionary<int, byte[]> cmds;

        public PlayerInputs(int capicity)
        {
            Capicity = capicity;
            cmds = new Dictionary<int, byte[]>(capicity);
        }

        public bool put(int id, byte[] content)
        {
            if (cmds.Count >= Capicity)
            {
                return true;
            }
            else
            {
                cmds.Add(id, content);
                return false;
            }
        }

        public override void Deserialize(ByteBuf buffer)
        {

        }

        public override void Serialize(ref ByteBuf buffer)
        {

        }

        public override byte[] GetBytes()
        {
            return null;
        }

        public override void DecodeBytes(byte[] bytes)
        {

        }
    }
}