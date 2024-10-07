using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtonManager : MonoBehaviour
{
    public void RestartScene()
    {
        Time.timeScale = 1.0f;
        LoadManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log(Time.timeScale);
    }

    public void OnLoadScene(string sceneName)
    {
        LoadManager.LoadScene(sceneName);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
