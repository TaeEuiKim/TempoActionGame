using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/*public class MonsterSkill : SkillBase<MonsterNormalSkillData>, ICooldownSkill
{
    private static float cooldownMultiplier = 0.01f;

    private float curTime; // seconds

    public MonsterSkill(MonsterNormalSkillData skillData) : base(skillData)
    {
        curTime = skillData.SkillCooldown;
        OnSkillAttack.AddListener((cb) => { Debug.Log("Invoke Normal Skill(Monster)"); });
    }

    public bool IsCooldown() => SkillRunner.SkillCooldown > curTime;

    public void UpdateTime(float deltaTime)
    {
        if (curTime > SkillRunner.SkillCooldown) { return; }

        curTime += deltaTime;
    }

    public override bool UseSkill(CharacterBase characterBase)
    {
        bool isRemove = false;
        if (IsCooldown()) { isRemove = true; }

        OnSkillAttack.Invoke(characterBase);

        curTime = 0;

        return isRemove;
    }
}
*/

public class MonsterSkill : SkillBase, ICooldownSkill
{
    public MonsterNormalSkillData skillData { get; private set; }

    private static float cooldownMultiplier = 0.01f;
    private float curTime; // seconds

    public MonsterSkill(SkillRunnerBase skillRunner) : base(skillRunner)
    {
        skillData = (MonsterNormalSkillData)skillRunner.skillData;
        curTime = skillData.SkillCooldown;
        OnSkillAttack.AddListener((cb) => { Debug.Log("Invoke Normal Skill(Monster)"); });
    }

    public bool IsCooldown() => skillData.SkillCooldown > curTime;

    public void UpdateTime(float deltaTime)
    {
        if (curTime > skillData.SkillCooldown) { return; }

        curTime += deltaTime;
    }

    public override bool UseSkill(CharacterBase character, UnityAction OnEnded = null)
    {
        if (IsCooldown()) { return false; }

        //OnSkillAttack.Invoke(characterBase);
        SkillRunner.Run(character, OnEnded);

        curTime = 0;

        return true;
    }

    public override int GetSkillId()
    {
        return -1;
    }
}
