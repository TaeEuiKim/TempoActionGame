using System;
using UnityEngine.Events;

[Serializable]
public abstract class SkillBase
{
    public int skillId { get;  protected set; }

    public string skillName { get; protected set; }

    public UnityEvent OnSkillAttack { get; protected set; }

    public SkillBase(int id, string name)
    {
        skillId = id;
        skillName = name;
        OnSkillAttack = new UnityEvent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>스킬이 소멸될지 여부. true = 소멸, false = 유지</returns>
    public abstract bool UseSkill();
}
