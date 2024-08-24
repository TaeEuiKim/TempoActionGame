using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Elite_Skill : ScriptableObject
{
    [SerializeField] protected Elite_SkillInfo _info;
    public Elite_SkillInfo Info { get => _info; }

    protected EliteMonster _monster;

    public virtual void Init(EliteMonster monster)
    {
        _monster = monster;
    }
    public abstract bool Check();
    public abstract void Enter();
    public abstract void Stay();
    public abstract void Exit();
}
