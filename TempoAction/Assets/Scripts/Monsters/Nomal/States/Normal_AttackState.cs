using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Normal_AttackState : Normal_State
{
    public Normal_AttackState(NormalMonster monster) : base(monster)
    {
    }

    public override void Stay()
    {
        if (_monster.Ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && _monster.Ani.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            _monster.Ani?.SetBool("Attack", false);
            _monster.ForceChangeState(Define.PerceptionType.IDLE);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _monster.Ani?.SetBool("Attack", false);
    }
}
