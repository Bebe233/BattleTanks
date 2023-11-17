using BEBE.Engine.Math;
using BEBE.Framework.Component;
using BEBE.Framework.Managers;
using BEBE.Framework.ULMath;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int Id { get; protected set; }
    public LFloat speed = 1;
    protected LVector3 targetPos;

    public abstract void ExecuteCmd(BInput binput);

    protected virtual void Awake()
    {
        targetPos = transform.position.ToLVector3();
    }

    //移动
    public void DoMove()
    {
        LVector3 currentPos = transform.position.ToLVector3();
        currentPos = LVector3.Lerp(currentPos, targetPos, 0.3f.ToLFloat());
        transform.position = currentPos.ToVector3();
    }

}
