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

    private void Start()
    {
        TestSound.Instance.PlaySound("Start");

        //_fire.SetActive(false);
        StartCoroutine(StartCutScene());
    }

    private IEnumerator StartCutScene()
    {
        float alpha = 0;

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < sprites.Length; i++)
        {
            if (i == 0)
            {
                while (sprites[i].color.a < 1)
                {
                    alpha += Time.fixedDeltaTime;
                    sprites[i].color = new Color(1, 1, 1, alpha);

                    yield return null;
                }
            }
            else
            {
                while (sprites[i].fillAmount < 1)
                {
                    sprites[i].fillAmount += Time.fixedDeltaTime;

                    yield return null;
                }
            }

            yield return new WaitForSeconds(3f);
        }

        sceneUI.SetActive(false);

        yield return new WaitForSeconds(1f);

        while (FadePanel.color.a > 0)
        {
            alpha -= Time.fixedDeltaTime;
            FadePanel.color = new Color(0, 0, 0, alpha);

            yield return null;
        }
    }
}
