using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Middle_PhaseEnd : Middle_PhaseState
{
    public Middle_PhaseEnd(MiddlePhaseManager manager) : base(manager)
    {

    }

    public override void Enter()
    {
        _manager.Monster2.Ani.SetBool("Death", true);

        CoroutineRunner.Instance.StartCoroutine(StartCutScene());
    }

    public override void Stay()
    {

    }

    public override void Exit()
    {

    }

    private IEnumerator StartCutScene()
    {
        float alpha = 0;

        yield return new WaitForSeconds(2f);

        _manager.FadePanel.gameObject.SetActive(true);

        while (_manager.FadePanel.color.a < 1)
        {
            alpha += Time.fixedDeltaTime;
            _manager.FadePanel.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        _manager.endSceneUI.SetActive(true);

        yield return new WaitForSeconds(2f);

        alpha = 0;

        for (int i = 0; i < _manager.endSprites.Length; i++)
        {
            if (i == 3)
            {
                while (_manager.endSprites[i].color.a < 1)
                {
                    alpha += Time.fixedDeltaTime;
                    _manager.endSprites[i].color = new Color(1, 1, 1, alpha);

                    yield return null;
                }
            }
            else
            {
                while (_manager.endSprites[i].fillAmount < 1)
                {
                    _manager.endSprites[i].fillAmount += Time.fixedDeltaTime;

                    yield return null;
                }
            }

            yield return new WaitForSeconds(2f);
        }
    }
}
