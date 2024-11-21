using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using FMOD;
using DG.Tweening;

public class CameraController : Singleton<CameraController>
{
    [Header("Component")]
    public GameObject player;
    public GameObject spawnPoint;
    public Image fadePanel;
    private bool isLook = false;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera _SceneCamera;
    [SerializeField] private CinemachineVirtualCamera[] _PlayerCamera;
    private CinemachineVirtualCamera _CurCamera;
    private Transform _cameraTrans;

    [Header("Camera Shaking")]
    [SerializeField] private float Impulse;
    [SerializeField] private float Frequency;
    private float shakeTime = 0f;

    [SerializeField] private NoiseSettings[] noiseSettings;

    private Quaternion saveCameraRotation;

    private MiddlePhaseManager middlePhaseManager;

    private void Awake()
    {
        middlePhaseManager = FindObjectOfType<MiddlePhaseManager>();
    }

    private void Update()
    {
        
    }

    public void TurnOnFadeOut(bool islook, string SceneName)
    {
        isLook = islook;
        _CurCamera = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        StartCoroutine(FadeOut(SceneName));
    }

    public void ChangeCamera(Define.CameraType state)
    {
        switch (state)
        {
            case Define.CameraType.PLAYER:
                _PlayerCamera[(int)Define.CameraType.PLAYER].GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z = -9.29f;
                break;
            case Define.CameraType.DOWN:
                break;
            case Define.CameraType.MIDDLEBOSS:
                TurnOnFadeOut(false, "START");
                break;
            case Define.CameraType.UNDERGROUND:
                TurnOnFadeOut(false, "UNDERGROUND");
                break;
            case Define.CameraType.NONE:
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
        else if (SceneName == "UNDERGROUND")
        {
            LoadManager.PassLoadScene("UndergroundStage");
        }

        yield break;
    }

    public void SetCameraSetting(Define.CameraType cameraType)
    {
        switch (cameraType)
        {
            case Define.CameraType.PLAYER:
                for (int i = 0; i < _PlayerCamera.Length; ++i)
                {
                    if (_PlayerCamera[i] != null && (int)Define.CameraType.PLAYER != i)
                    {
                        _PlayerCamera[i].gameObject.SetActive(false);
                    }
                }
                break;
            case Define.CameraType.DOWN:
                _PlayerCamera[(int)Define.CameraType.DOWN].gameObject.SetActive(true);
                break;
            case Define.CameraType.MIDDLEBOSS:
                break;
            case Define.CameraType.NONFOLLOW:
                _PlayerCamera[0].Follow = null;
                _PlayerCamera[0].transform.SetParent(null);
                break;
            case Define.CameraType.DEAD:
                _PlayerCamera[0].gameObject.SetActive(false);
                _PlayerCamera[2].gameObject.SetActive(true);
                break;
            case Define.CameraType.NONE:
                break;
        }
    }

    // Camera Shaking
    public void VibrateForTime(float times)
    {
        _CurCamera = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        if (SceneManager.GetActiveScene().name == "MiddleBossStage")
        {
            _CurCamera = _SceneCamera;
        }

        //_CurCamera.m_LookAt = null;
        shakeTime = times;
        StartCoroutine(CameraShaking());
    }

    IEnumerator CameraShaking()
    {
        CinemachineBasicMultiChannelPerlin ch = _CurCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        Player _player = player.GetComponent<Player>();
        int count = _player.Ani.GetInteger("CommandCount");
        if (count == 4 || count == 14 || count == 30)
        {
            ch.m_NoiseProfile = noiseSettings[0];
            _player.Ani.SetInteger("CommandCount", 0);
        }
        else
        {
            ch.m_NoiseProfile = noiseSettings[1];
        }

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

    public void SceneShaking()
    {
        _CurCamera = _SceneCamera;

        CinemachineBasicMultiChannelPerlin ch = _CurCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        ch.m_NoiseProfile = noiseSettings[1];

        StartCoroutine(SceneShakingTimer(ch));
    }

    private IEnumerator SceneShakingTimer(CinemachineBasicMultiChannelPerlin ch)
    {
        float time = 0.2f;

        while (time > 0)
        {
            ch.m_FrequencyGain = Frequency;
            ch.m_AmplitudeGain = Impulse;
            time -= Time.deltaTime;

            yield return null;
        }

        ch.m_FrequencyGain = 0;
        ch.m_AmplitudeGain = 0;

        yield return null;
    }

    public void SetCameraDamping(float amount)
    {
        if (_PlayerCamera[0] == null || !_PlayerCamera[0].gameObject.activeSelf)
        {
            return;
        }

        _PlayerCamera[0].GetCinemachineComponent<CinemachineTransposer>().m_XDamping = amount;
    }

    public void MoveSceneCamera(int moveCount)
    {
        float pos = 0;
        float time = 1f;
        if (moveCount == 0)
        {
            pos = 194.84f;
            _SceneCamera.transform.DOMoveX(pos, time);
        }
        else if (moveCount == 1)
        {
            pos = 193f;
            _SceneCamera.transform.DOMoveX(pos, time);
        }
        else if (moveCount == 2)
        {
            pos = 196.77f;
            _SceneCamera.transform.DOMoveX(pos, time);
        }
    }
}
