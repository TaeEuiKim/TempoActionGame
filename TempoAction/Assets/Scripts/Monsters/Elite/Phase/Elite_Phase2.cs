using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite_Phase2 : Elite_PhaseState
{
    public Elite_Phase2(ElitePhaseManager manager) : base(manager)
    {

    }

    public override void Enter()
    {
        _manager.Phase2Monster.gameObject.SetActive(true);
        _manager.Phase2Monster.Enter();
    }
    public override void Stay()
    {
        _manager.Phase2Monster.Stay();
    }

    public override void Exit()
    {

    }

   
}
