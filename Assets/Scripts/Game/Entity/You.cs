
using BEBE.Engine.Math;
using BEBE.Game.Inputs;
using UnityEngine;

public class You : PlayerEntity
{
    public override PlayerInput RecordCmd()
    {
        PlayerInput input = new PlayerInput((byte)Id);
        //获取移动的指令
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        input.x = x.ToLFloat();
        input.y = y.ToLFloat();

        return input;
    }
}