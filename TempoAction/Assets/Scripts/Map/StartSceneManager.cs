using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] public GameObject _fire;

    private void Start()
    {
        _fire.SetActive(false);
    }
}
