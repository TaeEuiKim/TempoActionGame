using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_SkillAttackState : Normal_State
{
    public Normal_SkillAttackState(NormalMonster monster) : base(monster) { }

    public override void Enter()
    {
        base.Enter();

        _monster.Rb.velocity = new Vector3(0, 0, 0);

        var slots = _monster.CurrentSkillSlots;
        if(slots.Length == 0) { Debug.LogError("Monster Skill Slot Length Is Zero."); }

        var slot = SelectSlot(slots);

        _monster.Direction = -((_monster.Target.transform.position - _monster.transform.position).x);

        if (slot.skillRunner.skillData.SkillId == 210)
        {
            _monster.Ani?.SetBool("Skill", true);
        }

        slot.UseSkillInstant(_monster, () =>
        {
            _monster.CurrentPerceptionState = Define.PerceptionType.IDLE;
        });
    }

    public override void Stay()
    {
    }

    public override void Exit()
    {
        base.Exit();

        _monster.CurrentSkillSlots = null;

        _monster.Ani?.SetBool("Skill", false);
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
