using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniFire : MonoBehaviour
{
    [SerializeField] private StartSceneManager _startSceneManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
        }
    }
}
