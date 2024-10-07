using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class NormalSkill : SkillBase, ICooldownSkill
{
    private static float cooldownMultiplier = 0.01f;

    private float curTime; // seconds

    private PlayerNormalSkillData skillData;

    private int skillId;

    // cooldown: 1/100 seconds
    public NormalSkill(SkillRunnerBase skillRunner) : base(skillRunner)
    {
        skillData = (PlayerNormalSkillData)skillRunner.skillData;
        curTime = skillData.SkillCooldown;
        skillId = skillData.SkillId;
        OnSkillAttack.AddListener((cb) => { Debug.Log("Invoke Normal Skill(Player)"); });
    }

    public virtual void UpdateTime(float deltaTime)
    {
        if (curTime > skillData.SkillCooldown) { return; }

        curTime += deltaTime;
    }

    public bool IsCooldown() => skillData.SkillCooldown > curTime;

    public override bool UseSkill(CharacterBase character, UnityAction OnEnded = null)
    {
        bool isRemove = false;
        if (IsCooldown()) { isRemove = true; }

        //OnSkillAttack.Invoke(sm);
        SkillRunner.Run(character, OnEnded);

        curTime = 0;

        return isRemove;
    }

    public override int GetSkillId()
    {
        return skillId;
    }
}
