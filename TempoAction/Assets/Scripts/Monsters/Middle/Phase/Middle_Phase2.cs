using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_Phase2 : Middle_PhaseState
{
    private float timer = 0;

    public Middle_Phase2(MiddlePhaseManager manager) : base(manager)
    {

    }

    public override void Enter()
    {
        _manager.Monster.phase = 2;

        CoroutineRunner.Instance.StartCoroutine(StartGCMove());
    }

    public override void Stay()
    {
        if (timer <= 3f)
        {
            timer += Time.deltaTime;
        }

        _manager.Monster.Stay();
        _manager.Monster2.Stay();

        if (_manager.GetHp() <= 0)
        {
            _manager.ChangeStageState(Define.MiddlePhaseState.FINISH);
        }
    }

    public override void Exit()
    {
    }

    private IEnumerator StartGCMove()
    {
        _manager.Monster.Ani.SetInteger("PhaseStartCount", 10);
        _manager.Monster.Ani.SetInteger("Phase", 2);

        yield return new WaitForSeconds(0.9f);

        _manager.Monster.transform.DOMoveY(30, 2.3f).OnComplete(() =>
        {
            _manager.Monster.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            _manager.Monster.transform.position = new Vector3(195.1f, 30, 12.58f);

            _manager.Monster.transform.DOMoveY(12f, 0.8f);
        });

        yield return new WaitForSeconds(1.5f);

        _manager.Monster.Ani.SetInteger("PhaseStartCount", 2);
    }
}
