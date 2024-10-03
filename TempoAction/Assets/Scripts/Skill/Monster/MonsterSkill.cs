using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : SkillBase<MonsterNormalSkillData>, ICooldownSkill
{
    private static float cooldownMultiplier = 0.01f;

    private float curTime; // seconds

    public MonsterSkill(MonsterNormalSkillData skillData) : base(skillData)
    {
        curTime = skillData.SkillCooldown;
        OnSkillAttack.AddListener((ISkillManager sm) => { Debug.Log("Invoke Normal Skill(Monster)"); });
    }

    public bool IsCooldown() => SkillData.SkillCooldown > curTime;

    public void UpdateTime(float deltaTime)
    {
        if (curTime > SkillData.SkillCooldown) { return; }

        curTime += deltaTime;
    }

    public override bool UseSkill(ISkillManager skillManager)
    {
        bool isRemove = false;
        if (IsCooldown()) { isRemove = true; }

        OnSkillAttack.Invoke(skillManager);

        curTime = 0;

        return isRemove;
    }
}
