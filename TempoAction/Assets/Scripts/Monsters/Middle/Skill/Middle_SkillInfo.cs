using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Middle_SkillInfo
{
    public Define.MiddleMonsterSkill skill;

    [Header("쿨타임")]
    public float coolTime;

    [Header("사거리 (경채는 조절X)")]
    public float range;

    [Header("데미지")]
    public float damage;

    [Header("스킬 우선순위")]
    public int priority;

    [Header("페이즈 구분")]
    public int level;
}