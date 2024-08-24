using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBall : MonoBehaviour
{
    public float TotalDamage { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(TotalDamage);

            GameObject explosion = ObjectPool.Instance.Spawn("ElectricBallExplosion");
            explosion.transform.position = transform.position;
            ObjectPool.Instance.Remove(gameObject);
        }
    }
}
