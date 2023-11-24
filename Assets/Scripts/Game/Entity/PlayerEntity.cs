using BEBE.Engine.Math;
using BEBE.Framework.ULMath;
using BEBE.Framework.Component;
public class PlayerEntity : Entity
{
    public float Speed = 0.382f;
    protected LFloat speed;
    float last_x_abs, last_y_abs;

    protected override void Awake()
    {
        base.Awake();
        speed = Speed.ToLFloat();
    }

    LVector3 last_dir;
    public override void ExecuteCmd(BInput binput)
    {
        base.ExecuteCmd(binput);
        var pinput = binput as PlayerInput;
        LFloat x = pinput.x;
        LFloat y = pinput.y;
        LFloat x_abs = LMath.Abs(x);
        LFloat y_abs = LMath.Abs(y);
        LVector3 dir = last_dir;
        if (x_abs > y_abs)
        {
            dir = LVector3.right * x;
        }
        else if (x_abs < y_abs)
        {
            dir = LVector3.up * y;
        }
        else
        {
            //根据上一帧的大小，取从小变大的那个
            if (last_x_abs < last_y_abs)
            {
                dir = LVector3.right * x;
            }
            else if (last_x_abs > last_y_abs)
            {
                dir = LVector3.up * y;
            }
            else
            {
                dir = new LVector3(dir.x * x_abs, dir.y * y_abs, 0);
            }
        }
        last_x_abs = x_abs;
        last_y_abs = y_abs;
        last_dir = dir;
        targetPos = transform.position.ToLVector3() + dir * speed;
        // transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }

    public override void RollbackCmd(BInput binput)
    {
        
    }
}