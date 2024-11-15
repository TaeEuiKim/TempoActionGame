using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    [Header("왼쪽에 나타나는 수")]
    [SerializeField] private int leftCount = 0;
    [Header("왼쪽에서 플레이어로부터 떨어져 나오는 거리")]
    [SerializeField] private float leftDir = 0;
    [Header("오른쪽에 나타나는 수")]
    [SerializeField] private int rightCount = 0;
    [Header("오른쪽에서 플레이어로부터 떨어져 나오는 거리")]
    [SerializeField] private float rightDir = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < leftCount; i++)
            {
                GameObject monster = ObjectPool.Instance.Spawn("NomalMonster3");
                monster.transform.position = other.transform.position + new Vector3(-leftDir + -i, 0);
                monster.GetComponent<NormalMonster>().IsTrace = true;
            }

            for (int i = 0; i < rightCount; i++)
            {
                GameObject monster = ObjectPool.Instance.Spawn("NomalMonster3");
                monster.transform.position = other.transform.position + new Vector3(rightDir + i, 0);
                monster.GetComponent<NormalMonster>().IsTrace = true;
            }

            GetComponent<Collider>().enabled = false;
        }
    }
}
