using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Dropbomb : MonoBehaviour
{
    public float TotalDamage;

    private float timer = 0f;
    private bool isGrounded = true;
    private Rigidbody rb;

    private Transform player;                // Ÿ���� Transform
    private GameObject mark;
    private float initialSpeed;              // �ʱ� �߻� �ӵ�
    private float guidanceStrength;          // ���� ����
    private float angle;
    private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
        timer = 0f;

        transform.rotation = Quaternion.Euler(0f, 0f, -180f);

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
            transform.up = -rb.velocity.normalized;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            DestoryMark();
            ObjectPool.Instance.Remove(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            isGrounded = true;
            DestoryMark();
            ObjectPool.Instance.Remove(this.gameObject);

            if (collision.gameObject.GetComponent<Player>().IsInvincible) return;

            collision.transform.GetComponent<Player>().TakeDamage(TotalDamage);
        }
    }

    public GameObject SettingValue(Transform player, float damage, GameObject mark, float initSpeed = 5f, float strength = 30f, float speed = 10f)
    {
        this.player = player;
        this.initialSpeed = initSpeed;
        this.guidanceStrength = strength;
        this.speed = speed;
        this.TotalDamage = damage;
        this.mark = mark;

        return gameObject;
    }

    private IEnumerator CheckTime()
    {
        while (initialSpeed == 0)
        {
            yield return new WaitForSeconds(0.01f);
        }

        Vector3 launchDirection = Vector3.up;
        rb.velocity = launchDirection * initialSpeed;

        yield return new WaitForSeconds(1f);

        isGrounded = false;
    }

    public void DestoryMark()
    {
        ObjectPool.Instance.Remove(mark);
    }
}
