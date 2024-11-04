using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] public GameObject _fire;
    [Header("ÄÆ¾À ¿ÀºêÁ§Æ®")]
    [SerializeField] public Image[] sprites;

    [Header("ÄÆ¾À UI")]
    [SerializeField] private GameObject sceneUI;

    [Header("ÆäÀÌµå ÆÐ³Î")]
    [SerializeField] private Image FadePanel;

    [Header("ÇÃ·¹ÀÌ¾î")]
    [SerializeField] private Player _player;

    private bool isCutScene;

    private void Start()
    {
        SetPlayerControll(true);

        StartCoroutine(StartCutScene());
    }

    private void LateUpdate()
    {
        if (PlayerInputManager.Instance.cancel && isCutScene)
        {
            PlayerInputManager.Instance.cancel = false;
            StopCoroutine(StartCutScene());
            sceneUI.SetActive(false);

            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator StartCutScene()
    {
        float alpha = 0;
        isCutScene = true;

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < sprites.Length; i++)
        {
            while (sprites[i].color.a < 1)
            {
                alpha += Time.fixedDeltaTime;
                sprites[i].color = new Color(1, 1, 1, alpha);

                yield return null;
            }

            yield return new WaitForSeconds(1f);

            alpha = 0;
        }

        sceneUI.SetActive(false);

        yield return new WaitForSeconds(1f);

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = 1;
        isCutScene = false;

        SetPlayerControll(false);

        while (FadePanel.color.a > 0)
        {
            alpha -= Time.fixedDeltaTime;
            FadePanel.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        TestSound.Instance.PlaySound("Start");

        yield return null;
    }

    private void SetPlayerControll(bool isKnockBack)
    {
        _player.PlayerSt.IsKnockedBack = isKnockBack;
    }
}
