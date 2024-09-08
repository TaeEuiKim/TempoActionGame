using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SkillSlot
{
    public ISkill skill { get; private set; }
    public UnityEvent<ISkill> OnRemoved = new UnityEvent<ISkill>();
    public KeyCode slotKey;

    public void SetSkill(ISkill newSkill)
    {
        skill = newSkill;
    }

    public void RemoveSkill()
    {
        OnRemoved.Invoke(skill);
        skill = null;
    }
}
