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
    }

    public override void Stay()
    {
        _monster.TrySkillAttack();

        float distance = Vector3.Distance(_monster.transform.position, _monster.Target.position);
        if(distance <= _monster.PerceptionDistance * SkillData.cm2m)
        {
            _monster.CurrentPerceptionState = Define.PerceptionType.TRACE;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
