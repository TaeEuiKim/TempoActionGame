using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shelling : MonoBehaviour
{
    public float TotalDamage { get; set; }
    public float MonsterDamage { get; set; }
    public Vector3 bombSize { get; set; }
    public LayerMask bombType { get; set; }

    private float timer = 0f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        timer = 0f;
        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            BombTimer();
            TestSound.Instance.PlaySound("Shelling_Boom");
        }
    }

    private void BombTimer()
    {
        Collider[] hitPlayer = Physics.OverlapBox(transform.position, bombSize / 2, transform.rotation, bombType);
        foreach (Collider collider in hitPlayer)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collider.GetComponent<Player>().TakeDamage(TotalDamage);
            }
            if (collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                collider.GetComponent<Monster>().TakeDamage(MonsterDamage);
            }
        }

        GameObject effect = ObjectPool.Instance.Spawn("BombEffect", 1);
        effect.transform.position = transform.position + new Vector3(0, -0.5f);
        CameraController.Instance.SceneShaking();
        ObjectPool.Instance.Remove(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, bombSize);
    }
}
