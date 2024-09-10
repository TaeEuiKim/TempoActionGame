using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject doorObj;
    public Image fadePanel;
    public GameObject spawnPoint;

    [Header("카메라")]
    [SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] CinemachineVirtualCamera playerCamera;

    [Header("카메라 관련 변수")]
    [SerializeField] Vector3 startFollowOffset;
    [SerializeField] Vector3 startTrackObjectOffset;

    [Header("카메라 쉐이킹 변수")]
    float shakeTime = 0f;
    [SerializeField] float Impulse;
    [SerializeField] float Frequency;

    private Quaternion saveCameraRotation;

    private void Start()
    {
        //SetStartCameraSetting();
    }

    public void TurnOnFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(FadeIn());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            RepairCamera();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            SetFollowLookObject(doorObj);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SetFollowLookPlayer();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            VibrateForTime(0.5f);
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

        camera.LookAt = player.transform;
        fadePanel.color = new Color(0, 0, 0, 0);
        yield break;
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float fadedTime = 0.5f;

        while (elapsedTime <= fadedTime)
        {
            fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(0f, 1f, elapsedTime / fadedTime));
            elapsedTime += Time.deltaTime;

            Debug.Log("FadeOut 중...");
            yield return null;
        }

        fadePanel.color = new Color(0, 0, 0, 1);

        camera.LookAt = null;
        player.transform.position = spawnPoint.transform.position;

        elapsedTime = 0;
        while (elapsedTime <= fadedTime)
        {

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        StartCoroutine(FadeIn());

        yield break;
    }

    private void SetFollowLookObject(GameObject obj)
    {
        saveCameraRotation = camera.transform.rotation;
        camera.m_LookAt = obj.transform;
        camera.m_Lens.FieldOfView = 70;
    }

    private void SetFollowLookPlayer()
    {
        camera.m_LookAt = player.transform;
        camera.transform.rotation = saveCameraRotation;
        camera.m_Lens.FieldOfView = 57;
    }

    private void SetStartCameraSetting()
    {
        camera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }

    private void RepairCamera()
    {
        playerCamera.gameObject.SetActive(false);
        camera.gameObject.SetActive(true);
    }

    // 카메라 쉐이킹
    public void VibrateForTime(float times)
    {
        camera.m_LookAt = null;
        shakeTime = times;
        StartCoroutine(CameraShaking());
    }

    IEnumerator CameraShaking()
    {
        while (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = Frequency;
            camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Impulse;
            yield return 0;
        }
        camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
        camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        camera.m_LookAt = player.transform;
        yield return 0;
    }
}
