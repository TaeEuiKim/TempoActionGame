using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : Stat
{
    private Animator _ani;

    void Start()
    {
        _ani = GetComponentInChildren<Animator>();
        _hp = _maxHp;
    }

    private void Update()
    {
        
        if (_isDead)
        {
            print("Á×À½");
        }
    }

    public void TakeDamage(float damage)
    {
        _ani.SetTrigger("IsHitting");
        Hp -= damage;
    }
}
