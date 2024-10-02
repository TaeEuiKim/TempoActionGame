using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shelling : MonoBehaviour
{
    public float TotalDamage { get; set; }
    public Vector3 bombSize { get; set; }
    public LayerMask bombType { get; set; }

    private float timer = 0f;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void LateUpdate()
    {
        if (transform.position.y > 27)
        {
            ObjectPool.Instance.Remove(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StartCoroutine(BombTimer());
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject effect = ObjectPool.Instance.Spawn("BombEffect", 1);
            effect.transform.position = transform.position;
            ObjectPool.Instance.Remove(this.gameObject);

            if (collision.gameObject.GetComponent<Player>().IsInvincible) return;

            collision.gameObject.GetComponent<Player>().TakeDamage(TotalDamage, true);
        }
    }

    IEnumerator BombTimer()
    {
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;

            yield return 0;
        }

        Collider[] hitPlayer = Physics.OverlapBox(transform.position, bombSize / 2, transform.rotation, bombType);
        if (hitPlayer.Length > 0)
        {
            Debug.Log(hitPlayer[0].gameObject.name);
            hitPlayer[0].GetComponent<Player>().TakeDamage(TotalDamage, true);
        }

        GameObject effect = ObjectPool.Instance.Spawn("BombEffect", 1);
        effect.transform.position = transform.position;
        ObjectPool.Instance.Remove(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, bombSize);
    }
}
