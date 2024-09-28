using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class SkillBase<T> : ISkillRoot where T : SkillData
{
    public T SkillData { get; protected set; }


    public UnityEvent<SkillManager> OnSkillAttack { get; protected set; }

    public SkillBase(T skillData)
    {
        SkillData = skillData;
        OnSkillAttack = new UnityEvent<SkillManager>();
    }

    public  abstract bool UseSkill(SkillManager skillManager);
}
