using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonManager : MonoBehaviour
{
    public void OnLoadScene(string sceneName)
    {
        LoadManager.LoadScene(sceneName);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
