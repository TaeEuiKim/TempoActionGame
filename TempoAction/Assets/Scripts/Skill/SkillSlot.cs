using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class SkillSlot
{
    public ISkillRoot Skill { get; protected set; }
    public UnityEvent<SkillSlot> OnRemoved = new UnityEvent<SkillSlot>();

    public virtual void SetSkill(ISkillRoot newSkill)
    {
        Skill = newSkill;

        newSkill?.SetSkillAdded();
    }

    public abstract void UseSkillInstant(CharacterBase character, UnityAction OnEnded = null);

    public void RemoveSkill()
    {
#if UNITY_EDITOR
        Debug.Log($"*½ºÅ³ ¼Ò¸ê({Skill.GetSkillId()})*");
#endif

        OnRemoved.Invoke(this);
        Skill = null;
    }
}