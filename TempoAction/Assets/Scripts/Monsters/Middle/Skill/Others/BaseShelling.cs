using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShelling : MonoBehaviour
{
    public Transform target;        // 목표 지점
    public float launchAngle = 45f; // 발사 각도
    public Rigidbody rb;            // Rigidbody 컴포넌트
    public float gravity = 9.8f;    // 중력 가속도 (Unity 기본값 사용 가능)

    private void Start()
    {
        // Rigidbody에 중력 설정
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // 목표 지점으로 투사체 발사
        LaunchProjectile();
    }

    private void LaunchProjectile()
    {
        // 목표 지점까지의 방향 벡터 계산
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.position;
        Vector3 displacement = targetPos - startPos;

        // 수평 거리와 높이 계산
        float horizontalDistance = new Vector2(displacement.x, displacement.z).magnitude;
        float verticalDistance = displacement.y;

        // 초기 속도 계산
        float angleInRadians = launchAngle * Mathf.Deg2Rad;
        float initialSpeed = Mathf.Sqrt((gravity * horizontalDistance * horizontalDistance) /
            (2 * (horizontalDistance * Mathf.Tan(angleInRadians) - verticalDistance)));

        // 초기 속도 벡터 계산
        Vector3 velocity = new Vector3(displacement.x, 0, displacement.z).normalized * initialSpeed;
        velocity.y = initialSpeed * Mathf.Sin(angleInRadians);

        // Rigidbody에 초기 속도 적용
        rb.velocity = velocity;
    }

    public void SetSetting(Transform target, float angle = 45f, float gravitySpeed = 9.8f)
    {
        this.target = target;
        this.launchAngle = angle;
        this.gravity = gravitySpeed;
    }
}
