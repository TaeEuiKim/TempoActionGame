using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using FMOD;

public class CameraController : MonoBehaviour
{
    [Header("Component")]
    public GameObject player;
    public GameObject spawnPoint;
    public Image fadePanel;
    private bool isLook = false;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera _SceneCamera;
    [SerializeField] private CinemachineVirtualCamera _PlayerCamera;
    private CinemachineVirtualCamera _CurCamera;

    [Header("Camera Shaking")]
    [SerializeField] private float Impulse;
    [SerializeField] private float Frequency;
    private float shakeTime = 0f;

    private Quaternion saveCameraRotation;

    private MiddlePhaseManager middlePhaseManager;

    private void Awake()
    {
        middlePhaseManager = FindObjectOfType<MiddlePhaseManager>();
    }

    public void TurnOnFadeOut(bool islook, string SceneName)
    {
        isLook = islook;
        _CurCamera = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        StartCoroutine(FadeOut(SceneName));
    }

    public void ChangeCamera(Define.MiddlePhaseState state)
    {
        switch (state)
        {
            case Define.MiddlePhaseState.START:
                TurnOnFadeOut(false, "START");
                //middlePhaseManager.ChangeStageState(state);
                break;
            case Define.MiddlePhaseState.PHASE1:
                break;
            case Define.MiddlePhaseState.PHASECHANGE:
                break;
            case Define.MiddlePhaseState.PHASE2:
                break;
            case Define.MiddlePhaseState.FINISH:
                break;
            case Define.MiddlePhaseState.NONE:
                break;
            default:
                break;
        }
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        float fadedTime = 0.5f;
    
        while (elapsedTime <= fadedTime)
        {
            fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(1f, 0f, elapsedTime / fadedTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if (isLook)
        {
            _CurCamera.LookAt = player.transform;
        }
        fadePanel.color = new Color(0, 0, 0, 0);
        isLook = false;
        yield break;
    }

    IEnumerator FadeOut(string SceneName)
    {
        float elapsedTime = 0f;
        float fadedTime = 0.5f;

        while (elapsedTime <= fadedTime)
        {
            fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(0f, 1f, elapsedTime / fadedTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        fadePanel.color = new Color(0, 0, 0, 1);

        if (isLook)
        {
            _CurCamera.LookAt = null;
        }

        elapsedTime = 0;
        while (elapsedTime <= fadedTime)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if (SceneName == "START")
        {
            LoadManager.LoadScene("MiddleBossStage");
        }

        yield break;
    }

    private void SetFollowLookObject(GameObject obj)
    {
        saveCameraRotation = _SceneCamera.transform.rotation;
        _SceneCamera.m_LookAt = obj.transform;
        _SceneCamera.m_Lens.FieldOfView = 70;
    }

    private void SetFollowLookPlayer()
    {
        _SceneCamera.m_LookAt = player.transform;
        _SceneCamera.transform.rotation = saveCameraRotation;
        _SceneCamera.m_Lens.FieldOfView = 57;
    }

    public void SetStartCameraSetting()
    {
        _SceneCamera.gameObject.SetActive(false);
        _PlayerCamera.gameObject.SetActive(true);
    }

    public void RepairCamera()
    {
        _PlayerCamera.gameObject.SetActive(false);
        _SceneCamera.gameObject.SetActive(true);
    }

    // Camera Shaking
    public void VibrateForTime(float times)
    {
        _CurCamera = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

        //_CurCamera.m_LookAt = null;
        shakeTime = times;
        StartCoroutine(CameraShaking());
    }

    IEnumerator CameraShaking()
    {
        CinemachineBasicMultiChannelPerlin ch = _CurCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        while (shakeTime > 0)
        {
            ch.m_FrequencyGain = Frequency;
            ch.m_AmplitudeGain = Impulse;
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        ch.m_FrequencyGain = 0;
        ch.m_AmplitudeGain = 0;
        //_CurCamera.m_LookAt = player.transform;
        yield return null;
    }
}
