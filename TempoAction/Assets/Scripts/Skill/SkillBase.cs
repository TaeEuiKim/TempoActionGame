using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class SkillBase
{
    [field:SerializeField] public int skillId { get;  protected set; }

    [field: SerializeField] public string skillName { get; protected set; }

    [field: SerializeField] public Define.SkillColliderType skillHitboxType { get; protected set; }
    [field: SerializeField] public float skillHitboxSize { get; protected set; }
    [field: SerializeField] public float skillDamage { get; protected set; }
    [field: SerializeField] public Define.SkillEffectType skillEffectType { get; protected set; }
    [field: SerializeField] public float skillEffectValue{ get; protected set; }


    public UnityEvent<SkillManager> OnSkillAttack { get; protected set; }

    public SkillBase(int id, string name, Define.SkillColliderType hitboxType, float hitboxSize, float damage, Define.SkillEffectType effectType, float effectValue)
    {
        skillId = id;
        skillName = name;
        skillHitboxType = hitboxType;
        skillHitboxSize = hitboxSize;
        skillDamage = damage;
        skillEffectType = effectType;
        skillEffectValue = effectValue;
        OnSkillAttack = new UnityEvent<SkillManager>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>스킬이 소멸될지 여부. true = 소멸, false = 유지</returns>
    public abstract bool UseSkill(SkillManager skillManager);
}
