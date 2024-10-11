using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainButtonManager : MonoBehaviour
{
    [SerializeField] private CopySkill copy;
    [Header("버튼 시스템")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] Button selectbutton;
    [Header("종료 UI")]
    [SerializeField] GameObject ExitMessage;

    private void Start()
    {
        copy = FindObjectOfType<CopySkill>();
        eventSystem = FindObjectOfType<EventSystem>(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Exit();

        #region 버튼 스크롤 이동
        Vector2 wheelInput2 = Input.mouseScrollDelta;

        Navigation SelectNavi = eventSystem.currentSelectedGameObject.GetComponent<Button>().navigation;

        if (wheelInput2.y > 0 && SelectNavi.selectOnUp)
            eventSystem.SetSelectedGameObject(SelectNavi.selectOnUp.gameObject);

        else if (wheelInput2.y < 0 && SelectNavi.selectOnDown)
            eventSystem.SetSelectedGameObject(SelectNavi.selectOnDown.gameObject);
        #endregion
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

    public void Exit()
    {
        ExitMessage.SetActive(!ExitMessage.activeSelf);
        if (ExitMessage.activeSelf) selectbutton.Select();
        else eventSystem.firstSelectedGameObject.GetComponent<Button>().Select();
    }
    public void OnExitGame()
    {
        Application.Quit();
    }
}
