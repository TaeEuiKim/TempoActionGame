using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : Stat
{
    private Animator _ani;

    void Start()
    {
        _ani = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float damage)
    {
        _ani.SetTrigger("IsHitting");
        Hp -= damage;
    }
}
