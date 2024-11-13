using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] public Image progressBar;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public static void PassLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        StartCoroutine(ChecKTimer(op));

        while (!op.isDone)
        {
            yield return null;

            if (progressBar.fillAmount < op.progress && progressBar.fillAmount < 0.9f)
            {
                progressBar.fillAmount += Time.fixedDeltaTime;
            }
            else if (op.progress >= 0.9f && progressBar.fillAmount >= 0.9f)
            {
                progressBar.fillAmount = 1f;
                if (progressBar.fillAmount >= 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

        yield return null;
    }

    // 로딩이 끝나지 않을 경우 예외처리
    IEnumerator ChecKTimer(AsyncOperation op)
    {
        float timer = 0f;

        while (timer <= 20f)
        {
            yield return null;

            timer += Time.unscaledDeltaTime;
        }

        if (op.isDone)
        {
            op.allowSceneActivation = true;
        }
    }
}
