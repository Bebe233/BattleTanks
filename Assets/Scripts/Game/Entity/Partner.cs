using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BEBE.Game.Inputs;

public class Partner : PlayerEntity
{
    public override PlayerInput RecordCmd()
    {
        return last_input ?? (last_input = new PlayerInput((byte)Id));
    }
}