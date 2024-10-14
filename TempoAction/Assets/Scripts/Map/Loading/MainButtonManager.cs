using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtonManager : MonoBehaviour
{
    [SerializeField] private CopySkill copy;

    private void Start()
    {
        copy = FindObjectOfType<CopySkill>();
    }

    public void RestartScene()
    {
        Time.timeScale = 1.0f;
        copy.SetSkillSlots();
        LoadManager.LoadScene("StartScene");
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
