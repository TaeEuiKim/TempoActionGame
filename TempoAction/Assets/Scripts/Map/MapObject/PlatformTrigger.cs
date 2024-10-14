using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField] Collider platformCollider;
    List<Collider> platformColliders = new List<Collider>();

    private bool isOn = false;

    private void OnTriggerEnter(Collider other)
    {
        platformCollider.isTrigger = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            Debug.Log(other.name);
            isOn = true;
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (other.GetComponent<Player>().Controller.isDown)
                {
                    GetComponent<Collider>().excludeLayers = 1 << 11;
                }
            }
        }
        else
        {
            isOn = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isOn)
        {
            platformCollider.isTrigger = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<Player>().Controller.isDown = false;
            GetComponent<Collider>().excludeLayers = 0;
        }
    }
}
