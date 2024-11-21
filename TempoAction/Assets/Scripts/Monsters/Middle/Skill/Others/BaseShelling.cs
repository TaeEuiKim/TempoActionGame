using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShelling : MonoBehaviour
{
    private Transform target;               // 목표 지점
    private float launchAngle = 45f;        // 발사 각도
    private Rigidbody rb;                   // Rigidbody 컴포넌트
    private float gravity = 9.8f;           // 중력 가속도
    private float launchForce = 1.5f;       // 발사 힘 배율

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void LaunchProjectile()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = target.position;

        Vector3 displacement = targetPosition - startPosition;
        float horizontalDistance = new Vector2(displacement.x, displacement.z).magnitude;
        float verticalDistance = displacement.y;

        float gravity = Physics.gravity.y * -1;
        float angleInRadians = launchAngle * Mathf.Deg2Rad;

        // 초기 속도 계산
        float initialSpeedSquared = (gravity * horizontalDistance * horizontalDistance) /
                                     (2 * (horizontalDistance * Mathf.Tan(angleInRadians) - verticalDistance));

        float initialSpeed = Mathf.Sqrt(initialSpeedSquared) * launchForce; // 배율 적용

        // 초기 속도 벡터 계산
        Vector3 horizontalDirection = new Vector3(displacement.x, 0, displacement.z).normalized;
        Vector3 velocity = horizontalDirection * initialSpeed * Mathf.Cos(angleInRadians);
        velocity.y = initialSpeed * Mathf.Sin(angleInRadians);

        rb.AddForce(velocity, ForceMode.VelocityChange);
    }

    public void SetSetting(Transform target, float launchForce = 1.5f, float angle = 45f, float gravitySpeed = 9.8f)
    {
        this.target = target;
        this.launchAngle = angle;
        this.gravity = gravitySpeed;
        this.launchForce = launchForce;

        LaunchProjectile();
    }
}
