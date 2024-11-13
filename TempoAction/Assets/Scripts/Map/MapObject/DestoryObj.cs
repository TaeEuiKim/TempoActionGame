using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryObj : BaseObject
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void LateUpdate()
    {
        if (Hp <= 0)
        {
            Ani.SetBool("Destory", true);
        }
    }

    private void DestroyObject()
    {
        ObjectPool.Instance.Remove(this.gameObject);
    }
}
