using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillCommand", menuName = "ScriptableObjects/Skill/SkillCommand/SkillCommand", order = 1)]
public class SkillCommand : ScriptableObject
{
    [SerializeField] public CommandData[] commandDatas;

    public float GetDamage(int skillCount)
    {
        return commandDatas.FirstOrDefault(data => data.SkillId == skillCount)?.damage ?? 0;
    }

    public float GetUltimateGauge(int skillCount)
    {
        return commandDatas.FirstOrDefault(data => data.SkillId == skillCount)?.fillUltimateGauge ?? 0;
    }
}

[Serializable]
public class CommandData
{
    [Header("스킬 이름")]
    public string name;
    [Header("스킬 아이디")]
    public int SkillId;
    [Header("연계 스킬 아이디")]
    public int[] PossibleSkillId;
    [Header("백 대쉬 연게 여부")]
    public bool IsBack;
    [Header("앞 대쉬 연계 여부")]
    public bool IsFront;
    [Header("스킬 커맨드")]
    public KeyCode[] KeyCodes;
    [Header("데미지")]
    public float damage;
    [Header("궁극기 게이지 채우는 량")]
    public float fillUltimateGauge;
}
