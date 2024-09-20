using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Dropbomb : MonoBehaviour
{
    public float TotalDamage;

    private float timer = 0f;
    private bool isGrounded = false;

    private void OnEnable()
    {
        timer = 0f;
        isGrounded = false;
    }

    private void LateUpdate()
    {
        if (isGrounded)
        {
            timer += Time.deltaTime;
            if (timer > 1.5f)
            {
                ObjectPool.Instance.Remove(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            ObjectPool.Instance.Remove(this.gameObject);

            if (collision.gameObject.GetComponent<Player>().IsInvincible) return;

            collision.transform.GetComponent<Player>().TakeDamage(TotalDamage);
        }
    }
}
