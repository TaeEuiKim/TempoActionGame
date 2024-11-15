using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    [SerializeField] private bool isLeft;

    private void OnTriggerEnter(Collider other)
    {
        if (isLeft)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                CameraController.Instance.MoveSceneCamera(1);
            }
        }
        else
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                CameraController.Instance.MoveSceneCamera(2);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CameraController.Instance.MoveSceneCamera(0);
        }
    }
}
