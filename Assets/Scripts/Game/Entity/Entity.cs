using BEBE.Engine.Math;
using BEBE.Framework.Component;
using BEBE.Framework.Managers;
using BEBE.Framework.ULMath;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int Id { get; set; }

    protected LVector3 targetPos;

    protected virtual void Awake()
    {
        targetPos = transform.position.ToLVector3();
        t = 0.618f.ToLFloat();
    }

    public virtual void ExecuteCmd(BInput binput)
    {
        translation();
    }

    public abstract void RollbackCmd(BInput binput);

    private LFloat t;
    protected void translation()
    {
        LVector3 currentPos = transform.position.ToLVector3();
        currentPos = LVector3.Lerp(currentPos, targetPos, t);
        transform.position = currentPos.ToVector3();
    }

}
