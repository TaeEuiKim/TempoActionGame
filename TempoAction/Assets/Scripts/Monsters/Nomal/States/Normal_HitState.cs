using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_HitState : Normal_State
{
    AnimatorStateInfo animatorStateInfo;

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
        _monster.Ani.SetBool("Attack", false);
    }

    public override void Stay() 
    {
        if (_monster.Ani.GetCurrentAnimatorStateInfo(0).length >= 0.56f && _monster.Ani.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))
        {
            _monster.Ani.SetBool("Hit", false);
            _monster.CurrentPerceptionState = Define.PerceptionType.IDLE;
            _monster.StartHitTimer();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
