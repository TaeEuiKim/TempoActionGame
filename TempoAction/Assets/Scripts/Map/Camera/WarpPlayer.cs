using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPlayer : MonoBehaviour
{
    [SerializeField] Define.WarpType warpType;
    private CameraController cameraController;
    private CopySkill copySkill;

    private void Awake()
    {
        copySkill = FindObjectOfType<CopySkill>();
        cameraController = Object.FindObjectOfType<CameraController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (warpType == Define.WarpType.UNDERGROUND)
            {
                Player player = other.GetComponent<Player>();
                cameraController.ChangeCamera(Define.CameraType.UNDERGROUND);
            }

            if (warpType == Define.WarpType.MIDDLEBOSS)
            {
                Player player = other.GetComponent<Player>();
                cameraController.ChangeCamera(Define.CameraType.MIDDLEBOSS);
            }
        }
    }
}
