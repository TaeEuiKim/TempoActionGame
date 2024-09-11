using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SkillSlot
{
    public SkillBase skill { get; private set; }
    public UnityEvent<SkillBase> OnRemoved = new UnityEvent<SkillBase>();
    public KeyCode slotKey;

    public void SetSkill(SkillBase newSkill)
    {
        skill = newSkill;
    }

    public void UseSkill()
    {
        if (skill.UseSkill())
        {
            RemoveSkill();
        }
    }

    public void RemoveSkill()
    {
        OnRemoved.Invoke(skill);
        skill = null;
    }
}
