using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSlash : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;

    public void OnFlip(Vector3 value)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.localScale = new Vector3(value.x, value.y, value.z);
        }
    }
}
