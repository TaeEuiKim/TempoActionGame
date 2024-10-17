using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Dropbomb : MonoBehaviour
{
    [SerializeField] private Vector3 ro;

    public float TotalDamage;
    private float monsterDamage;

    private float timer = 0f;
    private bool isGrounded = true;
    private Rigidbody rb;

    private Transform player;                // 타겟의 Transform
    private GameObject mark;
    private float initialSpeed;              // 초기 발사 속도
    private float guidanceStrength;          // 유도 강도
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

        //transform.rotation = Quaternion.Euler(0f, 0f, -180f);
        transform.rotation = Quaternion.Euler(ro);

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
            transform.up = -rb.velocity.normalized;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        DestoryMark();

        GameObject effect = ObjectPool.Instance.Spawn("TraceEffect", 1);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            effect.transform.position = collision.transform.position - new Vector3(0, 2.5f);
            collision.transform.GetComponent<Monster>().TakeDamage(monsterDamage);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            effect.transform.position = collision.transform.position - new Vector3(0, 2.5f);
            if (collision.gameObject.GetComponent<Player>().IsInvincible) return;

            collision.transform.GetComponent<Player>().TakeDamage(TotalDamage);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            effect.transform.position = transform.position - new Vector3(0, 3.5f);
        }

        ObjectPool.Instance.Remove(this.gameObject);
    }

    public GameObject SettingValue(Transform player, float damage, float monsterDamage, GameObject mark, float initSpeed = 5f, float strength = 30f, float speed = 10f)
    {
        this.player = player;
        this.initialSpeed = initSpeed;
        this.guidanceStrength = strength;
        this.speed = speed;
        this.TotalDamage = damage;
        this.mark = mark;
        this.monsterDamage = monsterDamage;

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
