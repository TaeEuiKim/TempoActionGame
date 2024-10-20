using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class SkillSlot
{
    public ISkillRoot Skill { get; protected set; }
    public UnityEvent<ISkillRoot> OnRemoved = new UnityEvent<ISkillRoot>();

    public virtual void SetSkill(ISkillRoot newSkill)
    {
        Skill = newSkill;

        newSkill?.SetSkillAdded();
    }

    public abstract void UseSkillInstant(CharacterBase character, UnityAction OnEnded = null);

    public void RemoveSkill()
    {
        OnRemoved.Invoke(Skill);
        Skill = null;
    }
}