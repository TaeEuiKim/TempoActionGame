using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSkill : SkillBase
{
    public InstantSkill(int id, string name, Define.SkillColliderType hitboxType, float hitboxSize, float damage, Define.SkillEffectType effectType, float effectValue) : base(id, name, hitboxType, hitboxSize, damage, effectType, effectValue)
    {
        OnSkillAttack.AddListener((SkillManager sm) => { Debug.Log("Invoke Instant Skill"); });
    }

    public override bool UseSkill(SkillManager sm)
    {
        OnSkillAttack.Invoke(sm);

        return true;
    }
}
