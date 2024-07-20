using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minFOV = 40f;
    [SerializeField] private float maxFOV = 60f;


    public void AdjustZoom(int direction)
    {
        float currentFOV = virtualCamera.m_Lens.FieldOfView;
        currentFOV -= direction * zoomSpeed;
        currentFOV = Mathf.Clamp(currentFOV, minFOV, maxFOV);
        virtualCamera.m_Lens.FieldOfView = currentFOV;
    }
}
