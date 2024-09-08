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
        // 스킬 슬롯에서 빈 자리 탐색
        for(int i = 0; i < skillSlots.Length; i++)
        {
            // 빈 곳이 있을 경우
            if (skillSlots[i] == null)
            {
                // 등록
                skillSlots[i].OnRemoved.AddListener(RemoveSkill);
                skillSlots[i].SetSkill(newSkill);
                return;
            }
        }
        
        // 자리가 없다면 큐(예비 스킬)에 등록
        reserveSlots.Enqueue(newSkill);
    }

    // 스킬 제거
    private void RemoveSkill(ISkill removedSkill)
    {
        // removedSkill 제거 과정
        int index = -1;
        for (index = 0; index < skillSlots.Length; index++)
        {
            if (skillSlots[index] == removedSkill)
            {
                skillSlots[index].OnRemoved.RemoveListener(RemoveSkill);
                skillSlots[index].RemoveSkill();
            }
        }

        // 예비 스킬 장착
        if (reserveSlots.Count == 0) { return; }
        
        ISkill nextSkill = reserveSlots.Dequeue();
        AddSkill(nextSkill);
    }
}
