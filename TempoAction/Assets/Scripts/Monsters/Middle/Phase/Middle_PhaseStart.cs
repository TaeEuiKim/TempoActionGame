using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Middle_PhaseStart : Middle_PhaseState
{
    private float startTime = 0.0f;

    public Middle_PhaseStart(MiddlePhaseManager manager) : base(manager)
    {

    }

    public override void Enter()
    {
        _manager.Phase1Monster.Enter();
        _manager.Phase1Monster2.Enter();
    }

    public override void Stay()
    {

    }

    public override void Exit()
    {

    }
}
