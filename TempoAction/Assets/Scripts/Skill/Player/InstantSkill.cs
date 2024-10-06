using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSkill : SkillBase<SkillData>
{
    public InstantSkill(SkillData skillData) : base(skillData)
    {
        OnSkillAttack.AddListener((cb) => { Debug.Log("Invoke Instant Skill"); });
    }

    public override bool UseSkill(CharacterBase cb)
    {
        OnSkillAttack.Invoke(cb);

        return true;
    }
}
