using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera camera;
    public GameObject player;
    public GameObject doorObj;
    public Image fadePanel;

    private Quaternion saveCameraRotation;

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

        if (Input.GetKeyDown(KeyCode.V))
        {
            SetFollowObject(doorObj);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SetFollowPlayer();
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

            Debug.Log("FadeIn 중...");
            yield return null;
        }

        fadePanel.color = new Color(0, 0, 0, 0);
        Debug.Log("FadeIn 끝");
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
        Debug.Log("FadeOut 끝");
        yield break;
    }

    private void SetFollowObject(GameObject obj)
    {
        saveCameraRotation = camera.transform.rotation;
        camera.m_LookAt = obj.transform;
        camera.m_Lens.FieldOfView = 70;
    }

    private void SetFollowPlayer()
    {
        camera.m_LookAt = player.transform;
        camera.transform.rotation = saveCameraRotation;
        camera.m_Lens.FieldOfView = 57;
    }
}
