using BEBE.Engine.Math;
using BEBE.Framework.ULMath;
using BEBE.Framework.Component;
using UnityEngine;
using UnityEngine.UI;
public class PlayerEntity : Entity
{
    //Shader : sequence of frames animation
    public Sprite[] key_frames;
    public int frame_per_second = 12;
    float last_x_abs, last_y_abs;

    Material m_material;
    BAnimation m_animation;

    protected override void Awake()
    {
        base.Awake();
        m_material = GetComponent<Image>().material;
        m_animation = new BAnimation(GetComponent<Image>(), key_frames, frame_per_second);
    }

    LVector3 last_dir;
    public override void ExecuteCmd(BInput binput)
    {
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
        // if (dir == LVector3.zero)
        // {
        //     m_animation.DoPause();
        //     return;
        // }
        targetPos = transform.position.ToLVector3() + dir * speed;
        // transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        //播放移动动画
        // m_animation.DoPlay();
    }

}