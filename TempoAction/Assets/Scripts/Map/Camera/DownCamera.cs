using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DownCamera : MonoBehaviour
{
    [Header("е╦ют")]
    [SerializeField] private Define.CameraType cameraType;
    private CameraController _cameraController;

    private void Awake()
    {
        _cameraController = FindObjectOfType<CameraController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (cameraType == Define.CameraType.DOWN)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _cameraController.SetCameraSetting(cameraType);
            }
        }
        else if (cameraType == Define.CameraType.PLAYER)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _cameraController.ChangeCamera(cameraType);
            }
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
