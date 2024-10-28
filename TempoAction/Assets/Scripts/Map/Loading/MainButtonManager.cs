using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtonManager : MonoBehaviour
{
    [SerializeField] private CopySkill copy;
    [SerializeField] private GameObject ExitUI;

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

    public void OnExitUI()
    {
        ExitUI.SetActive(true);
    }

    public void OffExitUI()
    {
        ExitUI.SetActive(false);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
