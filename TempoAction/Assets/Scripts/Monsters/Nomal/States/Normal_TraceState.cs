using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Normal_TraceState : Normal_State
{
    public Normal_TraceState(NormalMonster monster) : base(monster)
    {
    }

    public override void Enter()
    {
        base.Enter();

        
    }

    public override void Stay()
    {
        Vector3 dir = _monster.Target.transform.position - _monster.Target.transform.position;
        _monster.transform.position += dir * _monster.Stat.WalkSpeed * Time.deltaTime;
        _monster.Direction = dir.x;

        if (_monster.TrySkillAttack()) { return; }

        _monster.CurrentPerceptionState = Define.PerceptionType.GUARD;
    }
}
