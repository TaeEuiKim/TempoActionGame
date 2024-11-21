using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShelling : MonoBehaviour
{
    private Transform target;               // ��ǥ ����
    private float launchAngle = 45f;        // �߻� ����
    private Rigidbody rb;                   // Rigidbody ������Ʈ
    private float gravity = 9.8f;           // �߷� ���ӵ�
    private float launchForce = 1.5f;       // �߻� �� ����

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

        // �ʱ� �ӵ� ���
        float initialSpeedSquared = (gravity * horizontalDistance * horizontalDistance) /
                                     (2 * (horizontalDistance * Mathf.Tan(angleInRadians) - verticalDistance));

        float initialSpeed = Mathf.Sqrt(initialSpeedSquared) * launchForce; // ���� ����

        // �ʱ� �ӵ� ���� ���
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
