using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSkill : SkillBase<SkillData>
{
    public InstantSkill(SkillData skillData) : base(skillData)
    {
        OnSkillAttack.AddListener((ISkillManager sm) => { Debug.Log("Invoke Instant Skill"); });
    }

    public override bool UseSkill(ISkillManager sm)
    {
        OnSkillAttack.Invoke(sm);

        return true;
    }
}
