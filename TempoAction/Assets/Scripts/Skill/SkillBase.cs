using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
/*public abstract class SkillBase<T> : ISkillRoot where T : SkillData
{
    public T SkillRunner { get; protected set; }


    public UnityEvent<CharacterBase> OnSkillAttack { get; protected set; }

    public SkillBase(T skillData)
    {
        SkillRunner = skillData;
        OnSkillAttack = new UnityEvent<CharacterBase>();
    }

    public  abstract bool UseSkill(CharacterBase skillManager);
}
*/
public abstract class SkillBase : ISkillRoot
{ 
    public SkillRunnerBase SkillRunner { get; protected set; }

    public UnityEvent<CharacterBase> OnSkillAttack { get; protected set; }

    public SkillBase(SkillRunnerBase skillData)
    {
        SkillRunner = skillData;
        OnSkillAttack = new UnityEvent<CharacterBase>();
    }

    public abstract bool UseSkill(CharacterBase skillManager, UnityAction OnEnded = null);
}
