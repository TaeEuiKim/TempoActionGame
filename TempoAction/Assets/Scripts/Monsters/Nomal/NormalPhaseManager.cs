using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalPhaseManager : MonoBehaviour
{
    [SerializeField] private MonsterStat normalStat;
    [SerializeField] private Transform[] spawnPoints;

    private MonsterStat stat;

    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject monster = ObjectPool.Instance.Spawn("NomalMonster");
            stat = new MonsterStat();
            stat.MaxHp = normalStat.MaxHp;
            stat.WalkSpeed = normalStat.WalkSpeed;
            stat.SprintSpeed = normalStat.SprintSpeed;

            monster.GetComponent<NormalMonster>().Stat = stat;
            monster.transform.position = spawnPoints[i].position;
        }
    }
}
