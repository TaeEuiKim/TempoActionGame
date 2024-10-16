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

    private Transform player;                // 타겟의 Transform
    private float initialSpeed = 10f;        // 초기 발사 속도
    private float guidanceStrength = 5f;     // 유도 강도
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
            // 중력 적용
            rb.velocity += Vector3.up * Time.fixedDeltaTime;

            // 타겟 방향 계산
            Vector3 directionToTarget = (player.position - transform.position).normalized + new Vector3(0, 0.5f);

            // 유도 힘 계산 (목표 방향을 더 강하게 반영)
            Vector3 guidanceForce = directionToTarget * guidanceStrength;

            // 미사일 속도 조정 (유도 로직 추가)
            rb.velocity += guidanceForce * Time.fixedDeltaTime;

            // 최대 속도 제한
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);

            // 미사일의 앞 방향을 속도 벡터로 설정
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
