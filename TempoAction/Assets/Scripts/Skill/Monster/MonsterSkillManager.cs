using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSkillManager : MonoBehaviour, ISkillManager
{
    [field: SerializeField] public int MaxSkillSlot { get; protected set; }

    [field: SerializeReference] public SkillSlot[] SkillSlots {  get; protected set; }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (SkillSlots != null && SkillSlots?.Length != MaxSkillSlot)
        {
            var oldSlots = SkillSlots;
            var newSkillSlots = new MonsterSkillSlot[MaxSkillSlot];

            for (int i = 0; i < oldSlots.Length; i++)
            {
                if (i >= newSkillSlots.Length) { continue; }

                newSkillSlots[i] = oldSlots[i] as MonsterSkillSlot;
            }

            for(int i = oldSlots.Length; i < newSkillSlots.Length; i++)
            {
                newSkillSlots[i] = new MonsterSkillSlot();
            }

            SkillSlots = newSkillSlots;
        }

        for (int i = 0; i < SkillSlots.Length; i++)
        {
            var slot = SkillSlots[i] as MonsterSkillSlot;
            if (slot.skillRunner.skillData != null)
            {
                slot.SetSkill();
            }
        }
    }

    public MonsterSkillSlot[] GetUsableSkillSlots()
    {
        var slots = new List<MonsterSkillSlot>();

        for(int i = 0; i < SkillSlots.Length; i++)
        {
            if (SkillSlots[i] is MonsterSkillSlot slot)
            {
                if (slot.IsUsable(this)) {  slots.Add(slot); }
            }
        }

        return slots.ToArray();
    }

    public void OnUpdate(CharacterBase characterBase)
    {
        foreach(var slot in SkillSlots)
        {
            if(slot.Skill is  MonsterSkill skill)
            {
                skill.UpdateTime(Time.deltaTime);
            }
        }
    }
}