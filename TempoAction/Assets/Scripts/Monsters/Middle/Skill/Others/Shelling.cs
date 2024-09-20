using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelling : MonoBehaviour
{
    public float TotalDamage { get; set; }

    private void LateUpdate()
    {
        if (transform.position.y > 27)
        {
            ObjectPool.Instance.Remove(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ObjectPool.Instance.Remove(this.gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            ObjectPool.Instance.Remove(this.gameObject);

            if (other.gameObject.GetComponent<Player>().IsInvincible) return;

            other.GetComponent<Player>().TakeDamage(TotalDamage);
        }
    }
}
