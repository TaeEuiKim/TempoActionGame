using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Dropbomb : MonoBehaviour
{
    public float TotalDamage;

    private float timer = 0f;
    private bool isGrounded = true;
    private Rigidbody rb;

    private Transform player;                // Ÿ���� Transform
    private float initialSpeed = 10f;        // �ʱ� �߻� �ӵ�
    private float guidanceStrength = 5f;     // ���� ����
    private float angle;
    private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        timer = 0f;

        Vector3 launchDirection = Quaternion.Euler(-angle, 0, 0) * transform.forward;
        rb.velocity = launchDirection * initialSpeed;

        StartCoroutine(CheckTime());
    }

    private void LateUpdate()
    {
        if (!isGrounded)
        {
            // �߷� ����
            rb.velocity += Vector3.up * Time.fixedDeltaTime;

            // Ÿ�� ���� ���
            Vector3 directionToTarget = (player.position - transform.position).normalized + new Vector3(0, 0.5f);

            // ���� �� ��� (��ǥ ������ �� ���ϰ� �ݿ�)
            Vector3 guidanceForce = directionToTarget * guidanceStrength;

            // �̻��� �ӵ� ���� (���� ���� �߰�)
            rb.velocity += guidanceForce * Time.fixedDeltaTime;

            // �ִ� �ӵ� ����
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);

            // �̻����� �� ������ �ӵ� ���ͷ� ����
            transform.forward = rb.velocity.normalized;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            ObjectPool.Instance.Remove(this.gameObject);

            rb.velocity = Vector3.zero;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            isGrounded = true;
            ObjectPool.Instance.Remove(this.gameObject);

            if (collision.gameObject.GetComponent<Player>().IsInvincible) return;

            collision.transform.GetComponent<Player>().TakeDamage(TotalDamage);
            rb.velocity = Vector3.zero;
        }
    }

    public void SettingValue(Transform player, float initSpeed = 5f, float strength = 30f, float angle = 45f, float speed = 10f)
    {
        this.player = player;
        this.initialSpeed = initSpeed;
        this.guidanceStrength = strength;
        this.angle = angle;
        this.speed = speed;
    }

    private IEnumerator CheckTime()
    {
        yield return new WaitForSeconds(1f);

        isGrounded = false;
    }
}
