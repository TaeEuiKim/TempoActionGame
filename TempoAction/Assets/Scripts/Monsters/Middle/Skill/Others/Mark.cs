using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    public GameObject rocket;

    private void LateUpdate()
    {
        if (!rocket.activeInHierarchy)
        {
            ObjectPool.Instance.Remove(gameObject);
        }
    }
}
