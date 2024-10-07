using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/Skill/SkillData/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    [field: SerializeField] public int SkillId { get; protected set; }

    [field: SerializeField] public string SkillName { get; protected set; }
    [field: SerializeField] public float SkillCastingTime { get; protected set; } // 1/100sec
    [field: SerializeField] public float SkillRegenTime { get; protected set; } // 1/100sec
    [field: SerializeField] public Define.SkillType SkillClassType { get; protected set; } // 1/100sec
    [field: SerializeField] public Define.SkillColliderType SkillHitboxType { get; protected set; }
    [field: SerializeField] public float SkillHitboxSize { get; protected set; }
    [field: SerializeField] public Define.SkillTarget SkillCastingTarget{ get; protected set; }
    [field: SerializeField] public float SkillDamage { get; protected set; }
    [field: SerializeField] public int SkillAttackCount { get; protected set; }

    // 0: 추가 효과 X
    // 1: 전진/돌진
    [field: SerializeField] public Define.SkillEffectType SkillEffectType { get; protected set; }

    // SkillEffectType
    // 1: 이동 거리(cm)
    [field: SerializeField] public float SkillEffectValue { get; protected set; }

    public static float Time2Second = 0.01f;
    public static float cm2m = 0.01f;
}