using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Middle_Skill : ScriptableObject
{
    [SerializeField] protected Middle_SkillInfo _info;
    public Middle_SkillInfo Info { get => _info; }

    protected MiddleMonster _monster;
    public bool IsCompleted { get; set; }



    public virtual void Init(MiddleMonster monster)
    {
        _monster = monster;
        IsCompleted = false;
    }
    public abstract void Check();
    public abstract void Enter();

    public abstract void Stay();
    public abstract void Exit();

}
