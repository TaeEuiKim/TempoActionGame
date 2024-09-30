using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalSkillData : SkillData
{
    // seconds(0.01sec * multiplier)
    public float SkillCooldown
    {
        get { return skillCooldown * cooldownMultiplier; }
    }
    // cooldownMultiplier sec (현재 단위가 0.01s라는 뜻)
    [SerializeField] private float skillCooldown;

    public static float CooldownMultiplier { get { return cooldownMultiplier; } }

    public static float cooldownMultiplier = 0.01f;
}
