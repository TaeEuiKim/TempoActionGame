using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPlayer : MonoBehaviour
{
    [SerializeField] Define.WarpType warpType;
    private CameraController cameraController;

    private void Awake()
    {
        cameraController = Object.FindObjectOfType<CameraController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (warpType == Define.WarpType.MIDDLEBOSS)
            {
                cameraController.TurnOnFadeOut(false);
                cameraController.ChangeCamera(Define.MiddlePhaseState.START);
            }
        }
    }
}
