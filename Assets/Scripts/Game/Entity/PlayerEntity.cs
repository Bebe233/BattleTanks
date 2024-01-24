using BEBE.Engine.Math;
using BEBE.Framework.ULMath;
using BEBE.Framework.Component;
using BEBE.Game.Inputs;
using BEBE.Framework.Module;

public abstract class PlayerEntity : Entity
{
    public float Speed = 3f;
    protected LFloat speed;

    protected override void Awake()
    {
        base.Awake();
        speed = (Speed * (1f / Constant.TARGET_FRAME_RATE)).ToLFloat();
    }
    protected PlayerInput last_input;
    public override void ExecuteCmd(BInput binput)
    {
        var pinput = binput as PlayerInput;
        last_input = pinput;
        LFloat x = pinput.x;
        LFloat y = pinput.y;
        LVector3 dir = LVector3.zero;
        if (LMath.Abs(x) > 0)
        {
            dir = new LVector3(x, 0, 0);
        }
        else if (LMath.Abs(y) > 0)
        {
            dir = new LVector3(0, y, 0);
        }
        targetPos = transform.position.ToLVector3() + dir * speed;
        // transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        base.ExecuteCmd(binput);
    }

    public override void RollbackCmd(BInput binput)
    {

    }

    public abstract PlayerInput RecordCmd();
}