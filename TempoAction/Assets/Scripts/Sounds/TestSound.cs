using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundSources;
    [SerializeField] private string sceneName;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();

        switch (sceneName)
        {
            case "START":
                source.clip = soundSources[0];
                source.Play();
                break;
            default:
                break;
        }
    }
}
