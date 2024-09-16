using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_UseSkill : Middle_State
{
    public Middle_UseSkill(MiddleMonster monster) : base(monster)
    {

    }

    public override void Enter()
    {
    }

    public override void Stay()
    {
        _monster.CurrentSkill?.Stay();
    }

    public override void Exit()
    {
        _monster.ChangeCurrentSkill(Define.MiddleMonsterSkill.NONE);
        Debug.Log("Skill ³ª°¨");
    }
}
