using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DownCamera : MonoBehaviour
{
    private CameraController _cameraController;

    private void Awake()
    {
        _cameraController = FindObjectOfType<CameraController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _cameraController.SetCameraSetting(Define.CameraType.DOWN);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _cameraController.SetCameraSetting(Define.CameraType.PLAYER);
        }
    }
}
