using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using DG.Tweening;

public class Middle_PhaseStart : Middle_PhaseState
{
    private float timer = 0.0f;

    public Middle_PhaseStart(MiddlePhaseManager manager) : base(manager)
    {

    }

    public override void Enter()
    {
        _manager.Monster.Enter();
        _manager.Monster2.Enter();

        _manager.Monster.transform.DOMoveX(_manager._monsterSpawnPoint.position.x, 5f);
        _manager.Monster2.transform.DOMoveX(_manager._monster2SpawnPoint.position.x, 5f);
    }

    public override void Stay()
    {
        timer += Time.deltaTime;

        if (timer >= 3f)
        {
            _manager.ChangeStageState(Define.MiddlePhaseState.PHASE1);
        }
    }

    public override void Exit()
    {

    }
}
