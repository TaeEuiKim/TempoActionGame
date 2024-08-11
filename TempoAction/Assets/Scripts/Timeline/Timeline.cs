using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{
    [SerializeField] private string _name;
    public string Name { get => _name; }

    private PlayableDirector _director;
    public PlayableDirector Director { get=>_director; }

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();
    }
}
