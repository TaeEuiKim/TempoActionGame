using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_SkillAttackState : Normal_State
{
    public Normal_SkillAttackState(NormalMonster monster) : base(monster) { }

    public override void Enter()
    {
        base.Enter();

        var slots = _monster.CurrentSkillSlots;
        if(slots.Length == 0) { Debug.LogError("Monster Skill Slot Length Is Zero."); }

        var slot = SelectSlot(slots);
        slot.UseSkillInstant(_monster);
    }

    public override void Stay()
    {
    }

    private MonsterSkillSlot SelectSlot(MonsterSkillSlot[] slots)
    {
        MonsterSkillSlot selected = slots[0];
        var selectedSkillData = (MonsterNormalSkillData)selected.skillRunner.skillData;

        foreach (var slot in slots)
        {
            var skillData = (MonsterNormalSkillData)slot.skillRunner.skillData;
            if (skillData.SkillFiringValue > selectedSkillData.SkillFiringValue)
            {
                selected = slot;
                selectedSkillData = skillData;
            }
        }

        return selected;
    }
}
