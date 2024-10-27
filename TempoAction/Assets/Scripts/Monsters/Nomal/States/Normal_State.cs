using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Normal_State
{
    protected NormalMonster _monster;
    /*protected bool _isEntered;
    public bool IsEntered { get => _isEntered; }*/
    public Normal_State(NormalMonster monster)
    {
        _monster = monster;
    }

    public virtual void Enter()
    {
        //_isEntered = true;
    }
    public abstract void Stay();
    public virtual void Exit()
    {
        //_isEntered = false;
    }
}
