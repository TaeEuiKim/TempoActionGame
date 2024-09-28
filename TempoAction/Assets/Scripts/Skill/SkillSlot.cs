using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SkillSlot
{
    public ISkillRoot skill { get; private set; }
    public UnityEvent<ISkillRoot> OnRemoved = new UnityEvent<ISkillRoot>();
    public KeyCode slotKey;

    public void SetSkill(ISkillRoot newSkill)
    {
        skill = newSkill;
    }

    public void UseSkillInstant(SkillManager sm)
    {
        if(skill == null) { return; }

        if (skill.UseSkill(sm))
        {
            RemoveSkill();
        }
    }

    public void UseSkillKeyDown(SkillManager sm)
    {
        if(slotKey == KeyCode.None) { return; }

        if (Input.GetKeyDown(slotKey))
        {
            UseSkillInstant(sm);
        }
    }

    public void RemoveSkill()
    {
        OnRemoved.Invoke(skill);
        skill = null;
    }
}
