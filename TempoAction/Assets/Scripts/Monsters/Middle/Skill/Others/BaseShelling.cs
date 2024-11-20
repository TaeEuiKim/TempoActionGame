using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShelling : MonoBehaviour
{
    public Transform target;        // ��ǥ ����
    public float launchAngle = 45f; // �߻� ����
    public Rigidbody rb;            // Rigidbody ������Ʈ
    public float gravity = 9.8f;    // �߷� ���ӵ� (Unity �⺻�� ��� ����)

    private void Start()
    {
        // Rigidbody�� �߷� ����
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // ��ǥ �������� ����ü �߻�
        LaunchProjectile();
    }

    private void LaunchProjectile()
    {
        // ��ǥ ���������� ���� ���� ���
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.position;
        Vector3 displacement = targetPos - startPos;

        // ���� �Ÿ��� ���� ���
        float horizontalDistance = new Vector2(displacement.x, displacement.z).magnitude;
        float verticalDistance = displacement.y;

        // �ʱ� �ӵ� ���
        float angleInRadians = launchAngle * Mathf.Deg2Rad;
        float initialSpeed = Mathf.Sqrt((gravity * horizontalDistance * horizontalDistance) /
            (2 * (horizontalDistance * Mathf.Tan(angleInRadians) - verticalDistance)));

        // �ʱ� �ӵ� ���� ���
        Vector3 velocity = new Vector3(displacement.x, 0, displacement.z).normalized * initialSpeed;
        velocity.y = initialSpeed * Mathf.Sin(angleInRadians);

        // Rigidbody�� �ʱ� �ӵ� ����
        rb.velocity = velocity;
    }

    public void SetSetting(Transform target, float angle = 45f, float gravitySpeed = 9.8f)
    {
        this.target = target;
        this.launchAngle = angle;
        this.gravity = gravitySpeed;
    }
}
