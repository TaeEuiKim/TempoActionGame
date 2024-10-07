using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Middle_State
{
    protected MiddleMonster _monster;

    public Middle_State(MiddleMonster monster)
    {
        _monster = monster;
    }

    public abstract void Enter();


    public abstract void Stay();

    public abstract void Exit();

}
