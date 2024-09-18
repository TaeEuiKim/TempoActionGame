using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private int MaxSkillSlot;
    [SerializeField] private int MaxReserveSlot;

    [SerializeField] private SkillSlot[] skillSlots;
    private Queue<SkillBase> reserveSlots;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (skillSlots != null && skillSlots?.Length != MaxSkillSlot)
        {
            var oldSlots = skillSlots;
            skillSlots = new SkillSlot[MaxSkillSlot];

            for(int i = 0; i < oldSlots.Length; i++)
            {
                if(i >= skillSlots.Length) { continue; }

                skillSlots[i] = oldSlots[i];
            }
        }

        if (reserveSlots != null && reserveSlots?.Count != MaxReserveSlot)
        {
            var oldSlots = reserveSlots;
            reserveSlots = new Queue<SkillBase>(MaxReserveSlot);

            int oldSlotSize = oldSlots.Count;
            for (int i = 0; i < oldSlotSize; i++)
            {
                if (i >= MaxReserveSlot) { continue; }

                reserveSlots.Enqueue(oldSlots.Dequeue());
            }

            return;
        }

        reserveSlots = new Queue<SkillBase>(MaxReserveSlot); 
    }

    private void Update()
    {
        foreach (var slot in skillSlots)
        {
            slot.UseSkillKeyDown();
            if (slot.skill is NormalSkill normalSkill)
            {
                normalSkill.UpdateTime(Time.deltaTime);
            }
        }
    }

    public void AddSkill(SkillBase newSkill)
    {
        // ��ų ���Կ��� �� �ڸ� Ž��
        for(int i = 0; i < skillSlots.Length; i++)
        {
            // �� ���� ���� ���
            if (skillSlots[i] == null)
            {
                // ���
                skillSlots[i].OnRemoved.AddListener(RemoveSkill);
                skillSlots[i].SetSkill(newSkill);
                return;
            }
        }
        
        // �ڸ��� ���ٸ� ť(���� ��ų)�� ���
        reserveSlots.Enqueue(newSkill);
    }

    // ��ų ����
    private void RemoveSkill(SkillBase removedSkill)
    {
        // removedSkill ���� ����
        int index = -1;
        for (index = 0; index < skillSlots.Length; index++)
        {
            if (skillSlots[index].skill == removedSkill)
            {
                skillSlots[index].OnRemoved.RemoveListener(RemoveSkill);
                skillSlots[index].RemoveSkill();
            }
        }

        // ���� ��ų ����
        if (reserveSlots.Count == 0) { return; }
        
        SkillBase nextSkill = reserveSlots.Dequeue();
        AddSkill(nextSkill);
    }
}
