using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopySkill : MonoBehaviour
{
    private SkillSlot[] copySkillSlots;
    private Queue<ISkillRoot> copyReserveSlots;

    private void Start()
    {
    }

    public void SaveSkillSlots(SkillSlot[] skillSlots, Queue<ISkillRoot> reserveSlots)
    {
        copySkillSlots = skillSlots;
        copyReserveSlots = reserveSlots;
    }

    public void SetSkillSlots()
    {
        copySkillSlots = null;
        copyReserveSlots = null;
    }

    public SkillSlot[] LoadSkillSlots()
    {
        if (copySkillSlots == null)
        {
            return null;
        }
        return copySkillSlots;
    }

    public Queue<ISkillRoot> LoadReserveSlots()
    {
        return copyReserveSlots;
    }
}
