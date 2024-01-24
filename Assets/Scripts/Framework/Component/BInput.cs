using System.Collections.Generic;
using BEBE.Engine.Math;
using BEBE.Engine.Service.Cmd;
using BEBE.Engine.Service.Net;
using System;
using BEBE.Framework.Managers;

namespace BEBE.Framework.Component
{
    public abstract class BInput : Cmd, IEquatable<BInput>
    {
        public LFloat x, y; //axis raw value 摇杆/方向

        public override void Deserialize(ByteBuf buffer)
        {
            x = buffer.ReadLFloat();
            y = buffer.ReadLFloat();
        }

        public override void Serialize(ref ByteBuf buffer)
        {
            buffer.WriteLFloat(x);
            buffer.WriteLFloat(y);
        }

        public bool Equals(BInput other)
        {
            if (this.x != other.x) return false;
            if (this.y != other.y) return false;
            return true;
        }

    }

}