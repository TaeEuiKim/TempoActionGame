using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterSkillData
{
    public Define.SkillTerms SkillTriggerCondition { get; }
    public float SkillTriggerValue { get; }
    public float SkillFiringValue { get; }

    // SkillEffectType
    // 1: 이동 속도(m/s)
    // 4: 랜덤 생성 범위(cm, 1은 pc위치) -> 누구를 기준으로...??
    public float SkillEffectValue2 { get; }

     // 0: None
     // 1: 경직
     // 2: 넉백
     // 3: 쓰러짐
    public Define.SkillEffectType SkillSecondEffectType { get; }

    // SkillEffectType
    // 1: 경직 시간
    // 2: 넉백 거리
    public float SkillSecondEffectValue { get; }
}
