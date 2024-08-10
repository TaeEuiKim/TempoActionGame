using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{
    [SerializeField] private string _name;
    public string Name { get => _name; }

    public PlayableDirector Director { get; set; }

    private void Start()
    {
        Director = GetComponent<PlayableDirector>();
    }
}
