using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_Phase2 : Middle_PhaseState
{
    public Middle_Phase2(MiddlePhaseManager manager) : base(manager)
    {

    }

    public override void Enter()
    {
        _manager.Monster.phase = 2;
        _manager.Monster2.phase = 2;
    }

    public override void Stay()
    {
        _manager.Monster.Stay();
        _manager.Monster2.Stay();
    }

    public override void Exit()
    {
    }
}
