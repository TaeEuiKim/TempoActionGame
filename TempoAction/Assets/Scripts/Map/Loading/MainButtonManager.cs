using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtonManager : MonoBehaviour
{
    [SerializeField] private CopySkill copy;
    [SerializeField] private GameObject ExitUI;

    [Header("메인화면 전용")]
    [SerializeField] private GameObject[] idleImg;
    [SerializeField] private GameObject[] moveingImg;

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

    public void SetIdleImg(int arrayNum)
    {
        idleImg[arrayNum].SetActive(true);
        moveingImg[arrayNum].SetActive(false);
    }

    public void SetMovingImg(int arrayNum)
    {
        idleImg[arrayNum].SetActive(false);
        moveingImg[arrayNum].SetActive(true);
    }
}
