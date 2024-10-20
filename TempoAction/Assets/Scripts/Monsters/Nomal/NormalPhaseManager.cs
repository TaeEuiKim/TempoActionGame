using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalPhaseManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject monster = ObjectPool.Instance.Spawn("NomalMonster");

            monster.transform.position = spawnPoints[i].position;
        }
    }
}
