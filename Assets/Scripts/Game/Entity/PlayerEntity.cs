using UnityEngine;

public class PlayerEntity : Entity
{
    bool locker = false;
    float last_x_abs, last_y_abs;
    Vector3 last_dir;
    protected void GetCommand()
    {
        //获取移动的指令
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        float x_abs = Mathf.Abs(x);
        float y_abs = Mathf.Abs(y);
        Vector3 dir = last_dir;
        if (x_abs > y_abs)
        {
            dir = Vector3.right * x;
        }
        else if (x_abs < y_abs)
        {
            dir = Vector3.up * y;
        }
        else
        {
            //根据上一帧的大小，取从小变大的那个
            if (last_x_abs < last_y_abs)
            {
                dir = Vector3.right * x;
            }
            else if (last_x_abs > last_y_abs)
            {
                dir = Vector3.up * y;
            }
            else
            {
                dir = new Vector3(dir.x * x_abs, dir.y * y_abs, 0);
            }
        }
        last_x_abs = x_abs;
        last_y_abs = y_abs;
        last_dir = dir;
        if (dir == Vector3.zero) return;
        targetPos = transform.position + dir * speed;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }

    private void Update()
    {
        GetCommand();
    }
   
}