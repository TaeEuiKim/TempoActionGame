using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SkillSlot
{
    public ISkillRoot Skill { get; protected set; }
    public UnityEvent<ISkillRoot> OnRemoved = new UnityEvent<ISkillRoot>();

    public void SetSkill(ISkillRoot newSkill)
    {
        Skill = newSkill;
    }

    public void UseSkillInstant(ISkillManager sm)
    {
        if (Skill == null) { return; }

        if (Skill.UseSkill(sm))
        {
            RemoveSkill();
        }
    }

    public void RemoveSkill()
    {
        OnRemoved.Invoke(Skill);
        Skill = null;
    }
}