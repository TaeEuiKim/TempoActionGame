using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Middle_PhaseEnd : Middle_PhaseState
{
    public Middle_PhaseEnd(MiddlePhaseManager manager) : base(manager)
    {

    }

    public override void Enter()
    {
        _manager.Monster.Ani.SetBool("Die", true);
        _manager.Monster2.Ani.SetBool("Death", true);

        TestSound.Instance.StopBGMSound("MiddleBGM");
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
        _manager.Monster.Player.GetComponent<Player>().View.UiEffect.SetActive(false);
        _manager.Monster.Player.GetComponent<Player>().View.isUltimate = true;

        yield return new WaitForSeconds(2f);

        _manager.FadePanel.gameObject.SetActive(true);

        while (_manager.FadePanel.color.a < 1)
        {
            alpha += Time.fixedDeltaTime;
            _manager.FadePanel.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        _manager.KOPanel.SetActive(true);

        yield return new WaitForSeconds(2f);

        _manager.KOPanel.SetActive(false);

        yield return new WaitForSeconds(1f);

        _manager.endSceneUI.SetActive(true);

        yield return new WaitForSeconds(2f);


        for (int i = 0; i < _manager.endSprites.Length; i++)
        {
            alpha = 0;
            while (_manager.endSprites[i].color.a < 1)
            {
                alpha += Time.fixedDeltaTime;
                _manager.endSprites[i].color = new Color(1, 1, 1, alpha);

                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(2f);

        _manager.endSceneUI.SetActive(false);

        _manager.credit.SetActive(true);

        yield return new WaitForSeconds(30f);

        LoadManager.LoadScene("MainScene");
    }
}
