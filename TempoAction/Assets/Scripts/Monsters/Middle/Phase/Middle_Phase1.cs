using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_Phase1 : Middle_PhaseState
{

    public Middle_Phase1(MiddlePhaseManager manager) : base(manager)
    {

    }

    public override void Enter()
    {
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
