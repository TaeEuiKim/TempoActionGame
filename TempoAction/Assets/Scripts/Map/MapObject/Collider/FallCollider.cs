using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCollider : MonoBehaviour
{
    [Header("스폰 포인트")]
    [SerializeField] private Transform point;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.position = point.position;
            other.GetComponent<Player>().TakeDamage(20f);
        }
    }
}
