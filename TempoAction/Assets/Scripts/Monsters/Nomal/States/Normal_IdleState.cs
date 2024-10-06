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
        var slots = _monster._SkillManager.GetUsableSkillSlots();

        if(slots.Count() > 0)
        {
            _monster.CurrentSkillSlots = slots;
            _monster.CurrentPerceptionState = Define.PerceptionType.SKILLATTACK;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
