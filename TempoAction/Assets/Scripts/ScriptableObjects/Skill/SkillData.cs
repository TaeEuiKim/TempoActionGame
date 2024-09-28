using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    [field: SerializeField] public int SkillId { get; protected set; }

    [field: SerializeField] public string SkillName { get; protected set; }

    [field: SerializeField] public Define.SkillColliderType SkillHitboxType { get; protected set; }
    [field: SerializeField] public float SkillHitboxSize { get; protected set; }
    [field: SerializeField] public float SkillDamage { get; protected set; }
    [field: SerializeField] public Define.SkillEffectType SkillEffectType { get; protected set; }
    [field: SerializeField] public float SkillEffectValue { get; protected set; }
}