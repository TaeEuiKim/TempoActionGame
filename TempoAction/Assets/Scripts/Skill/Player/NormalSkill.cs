using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class NormalSkill : SkillBase, ICooldownSkill
{
    private static float cooldownMultiplier = 0.01f;

    private float curTime; // seconds

    private PlayerNormalSkillData skillData;

    private int skillId;

    public int SkillCountCharged { get; private set; }
    // cooldown: 1/100 seconds
    public NormalSkill(SkillRunnerBase skillRunner) : base(skillRunner)
    {
        skillData = (PlayerNormalSkillData)skillRunner.skillData;
        curTime = skillData.SkillCooldown;
        skillId = skillData.SkillId;
        OnSkillAttack.AddListener((cb) => { Debug.Log("Invoke Normal Skill(Player)"); });
    }

    public override void SetSkillAdded()
    {
        SkillCountCharged = skillData.SkillMaxLimit;
    }

    public virtual void UpdateTime(float deltaTime)
    {
        if (curTime > skillData.SkillCooldown) { return; }

        curTime += deltaTime;
    }

    public bool IsCooldown() => skillData.SkillCooldown > curTime;

    public override void UseSkill(CharacterBase character, UnityAction OnEnded = null)
    {
        if (IsCooldown()) { UseSkillCount(); }

        //OnSkillAttack.Invoke(sm);
        SkillRunner.Run(this, character, OnEnded);

        curTime = 0;
    }
    public override int GetSkillId()
    {
        return skillId;
    }
    void UseSkillCount()
    {
        SkillCountCharged--;
    }
}
