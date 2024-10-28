using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Normal_IdleState : Normal_State
{
    public Normal_IdleState(NormalMonster monster) : base(monster) { }

    public override void Enter()
    {
        base.Enter();
        _monster.Ani.SetBool("RunStop", false);
    }

    public override void Stay()
    {
        _monster.Direction = -(_monster.Player.position.x - _monster.transform.position.x);

        float distance = Vector3.Distance(_monster.transform.position, _monster.Target.position);

        if (_monster.TryAttack())
        {
            if (_monster.Ani.GetBool("Attack"))
            {
                _monster.Ani.SetBool("Attack", false);
            }
        }
        // 공격 범위 밖, 인지 범위 내(추격)
        else if (distance > _monster.MonsterSt.AttackRange && distance <= _monster.PerceptionDistance * SkillData.cm2m)
        {
            _monster.CurrentPerceptionState = Define.PerceptionType.TRACE;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
