using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TS : ISkill
{
    string name;
    public TS(string name)
    {
        this.name = name;
    }   
}

public class SkillManager : MonoBehaviour
{
    [SerializeField] private int MaxSkillSlot;
    [SerializeField] private int MaxReserveSlot;

    [SerializeField] private SkillSlot[] skillSlots;
    private Queue<ISkill> reserveSlots;

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
            reserveSlots = new Queue<ISkill>(MaxReserveSlot);

            int oldSlotSize = oldSlots.Count;
            for (int i = 0; i < oldSlotSize; i++)
            {
                if (i >= MaxReserveSlot) { continue; }

                reserveSlots.Enqueue(oldSlots.Dequeue());
            }

            return;
        }

        reserveSlots = new Queue<ISkill>(MaxReserveSlot); 
    }

    public void AddSkill(ISkill newSkill)
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
    private void RemoveSkill(ISkill removedSkill)
    {
        // removedSkill ���� ����
        int index = -1;
        for (index = 0; index < skillSlots.Length; index++)
        {
            if (skillSlots[index] == removedSkill)
            {
                skillSlots[index].OnRemoved.RemoveListener(RemoveSkill);
                skillSlots[index].RemoveSkill();
            }
        }

        // ���� ��ų ����
        if (reserveSlots.Count == 0) { return; }
        
        ISkill nextSkill = reserveSlots.Dequeue();
        AddSkill(nextSkill);
    }
}
