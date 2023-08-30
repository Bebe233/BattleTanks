using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float speed = 1;
    protected Vector3 targetPos;

    private void Awake()
    {
        targetPos = transform.position;
    }
    //移动
    public void DoMove()
    {
        Vector3 currentPos = transform.position;
        currentPos = Vector3.Lerp(currentPos, targetPos, 0.3f);
        transform.position = currentPos;

    }

    protected virtual void FixedUpdate()
    {
        DoMove();
    }


    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Chunk"))
        {
            print("OnTrigger Chunk!");
            //获取方向
            
        }
    }
}
