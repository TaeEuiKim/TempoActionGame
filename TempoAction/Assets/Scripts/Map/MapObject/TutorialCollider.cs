using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    StartSceneManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<StartSceneManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
