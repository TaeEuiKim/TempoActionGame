using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

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

        _manager.Monster2.Ani.SetBool("Walk", true);

        _manager.Monster.Ani.SetInteger("PhaseStartCount", 1);
        _manager.Monster.Ani.SetInteger("Phase", 1);

        CoroutineRunner.Instance.StartCoroutine(StartGCMove());
        _manager.Monster2.transform.DOMoveX(_manager._middlePoint[Define.MiddleMonsterPoint.CSPAWNPOINT].position.x, 3f);

        TestSound.Instance.PlaySound("MiddleBGM");
    }

    public override void Stay()
    {
        timer += Time.deltaTime;

        if (timer >= 3f)
        {
            _manager.Monster2.Ani.SetBool("Walk", false);
            _manager.ChangeStageState(Define.MiddlePhaseState.PHASE1);
        }
    }

    public override void Exit()
    {

    }

    private IEnumerator StartGCMove()
    {
        yield return new WaitForSeconds(0.5f);

        _manager.Monster.transform.DOMoveY(30, 2.3f).OnComplete(() =>
        {
            _manager.Monster.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            _manager.Monster.transform.position = new Vector3(_manager._middlePoint[Define.MiddleMonsterPoint.CSPAWNPOINT].position.x + 1.5f,
                                                              _manager.Monster.transform.position.y, _manager._middlePoint[Define.MiddleMonsterPoint.RIGHTSIDE].position.z);
            _manager.Monster.transform.DOMoveY(0.8f, 0.7f);
        });

        yield return new WaitForSeconds(2f);

        _manager.Monster.Ani.SetInteger("PhaseStartCount", 2);
    }
}
