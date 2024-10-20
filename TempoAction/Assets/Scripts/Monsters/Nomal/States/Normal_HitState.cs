using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_HitState : Normal_State
{
    public Normal_HitState(NormalMonster monster) : base(monster) { }

    public override void Enter()
    {
        base.Enter();

        if (_monster.Stat.Hp <= 0)
        {
            return;
        }

        // 애니 적용
        if (!_monster.Ani.GetBool("Hit"))
        {
            _monster.Ani.SetBool("Hit", true);
        }
    }

    public override void Stay() 
    {
    }

    public override void Exit()
    {
        base.Exit();

        _monster.Ani.SetBool("Hit", false);
    }
}
