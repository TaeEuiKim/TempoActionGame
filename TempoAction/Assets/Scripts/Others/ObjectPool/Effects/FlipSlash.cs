using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSlash : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;

    public void OnFlip(Vector3 value)
    {
        objects[0].transform.localScale = new Vector3(value.x, 1, 1);
        objects[1].transform.localScale = new Vector3(value.x, 1, 1);
        objects[2].transform.localScale = new Vector3(value.x, 1, 1);
    }
}
