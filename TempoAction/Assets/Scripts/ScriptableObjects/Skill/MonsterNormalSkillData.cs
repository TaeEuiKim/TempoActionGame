using UnityEngine;

[CreateAssetMenu(fileName = "MonsterNormalSkillData", menuName = "ScriptableObjects/Skill/SkillData/Normal/MonsterNormalSkillData", order = 1)]
public class MonsterNormalSkillData : NormalSkillData, IMonsterSkillData
{
    [field: SerializeField] public Define.SkillTerms SkillTriggerCondition { get; protected set; }
    [field: SerializeField] public float SkillTriggerValue { get; protected set; }
    [field: SerializeField] public float SkillFiringValue { get; protected set; }
    [field: SerializeField] public float SkillEffectValue2 { get; protected set; }
    [field: SerializeField] public Define.SkillEffectType SkillSecondEffectType { get; protected set; }
    [field: SerializeField] public float SkillSecondEffectValue { get; protected set; }
}