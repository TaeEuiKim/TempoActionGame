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
        curTime += deltaTime;

        if (curTime > skillData.SkillCooldown)
        {
            RechargeSkillCount();
            curTime = 0;
        }
    }

    public bool IsSkillUsable() => SkillCountCharged > 0;

    public override void UseSkill(CharacterBase character, UnityAction OnEnded = null)
    {
        if (IsSkillUsable()) { UseSkillCount(); }

        //OnSkillAttack.Invoke(sm);
        SkillRunner.Run(this, character, OnEnded);
    }

    public override int GetSkillId()
    {
        return skillId;
    }

    public void UseSkillCount()
    {
        SkillCountCharged--;
        SetSkillCountInRange();
    }

    private void RechargeSkillCount()
    {
        SkillCountCharged++;
        SetSkillCountInRange();
    }

    private void SetSkillCountInRange()
    {
        SkillCountCharged = Mathf.Clamp(SkillCountCharged, 0, skillData.SkillMaxLimit);
    }
}
