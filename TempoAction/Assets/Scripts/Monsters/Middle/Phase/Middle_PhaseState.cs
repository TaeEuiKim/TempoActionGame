using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Middle_PhaseState
{
    protected MiddlePhaseManager _manager;
    public Middle_PhaseState(MiddlePhaseManager manager)
    {
        _manager = manager;
    }

    public abstract void Enter();
    public abstract void Stay();
    public abstract void Exit();
}
