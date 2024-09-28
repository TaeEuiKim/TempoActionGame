using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSkill : SkillBase<SkillData>
{
    public InstantSkill(SkillData skillData) : base(skillData)
    {
        OnSkillAttack.AddListener((SkillManager sm) => { Debug.Log("Invoke Instant Skill"); });
    }

    public override bool UseSkill(SkillManager sm)
    {
        OnSkillAttack.Invoke(sm);

        return true;
    }
}
