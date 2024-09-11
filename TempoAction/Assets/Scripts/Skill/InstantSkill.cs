using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSkill : SkillBase
{
    public InstantSkill(int id) : base(id)
    {
        OnSkillAttack.AddListener(() => { Debug.Log("Invoke Instant Skill"); });
    }

    public override bool UseSkill()
    {
        OnSkillAttack.Invoke();

        return true;
    }
}
