using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_SkillAttackState : Normal_AttackState
{
    public Normal_SkillAttackState(NormalMonster monster) : base(monster) { }

    public override void Enter()
    {
        base.Enter();

        _monster.Ani.SetBool("Attack", false);
        _monster.Ani.SetBool("Run", false);

        _monster.Rb.velocity = new Vector3(0, 0, 0);

        var slots = _monster.CurrentSkillSlots;
        if(slots.Length == 0) { Debug.LogError("Monster Skill Slot Length Is Zero."); }

        var slot = SelectSlot(slots);

        if (_monster.monsterType == Define.NormalMonsterType.BALDO)
        {
            _monster.Direction = -((_monster.Target.transform.position - _monster.transform.position).x);
        }
        else if (_monster.monsterType == Define.NormalMonsterType.KUNG)
        {
            _monster.Direction = (_monster.Target.transform.position - _monster.transform.position).x;
        }

        _monster.Ani?.SetBool("Skill", true);

        slot.UseSkillInstant(_monster);
    }

    public override void Stay()
    {
        base.Stay();
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
