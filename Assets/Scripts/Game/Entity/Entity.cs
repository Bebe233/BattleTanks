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
        t = 0.382f.ToLFloat();
    }

    public abstract void ExecuteCmd(BInput binput);

    public virtual void DoRender()
    {
        translation();
    }

    private LFloat t;
    protected void translation()
    {
        LVector3 currentPos = transform.position.ToLVector3();
        currentPos = LVector3.Lerp(currentPos, targetPos, t);
        transform.position = currentPos.ToVector3();
    }

}
