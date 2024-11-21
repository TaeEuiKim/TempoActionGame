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
        _manager.Monster.phase = 1;
        _manager.Monster2.phase = 1;
    }

    public override void Stay()
    {
        _manager.Monster.Stay();
        _manager.Monster2.Stay();

        if (_manager.Monster2.MonsterSt.Hp <= 0)
        {
            _manager.ChangeStageState(Define.MiddlePhaseState.FINISH);
        }
    }

    public override void Exit()
    {

    }
}
