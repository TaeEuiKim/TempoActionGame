using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Dropbomb", menuName = "ScriptableObjects/MiddleMonster/Skill/Dropbomb", order = 1)]
public class Middle_Dropbomb : Middle_Skill
{
    [SerializeField] private int bombAmount;        // ��ź ����
    [SerializeField] private float launchAngle = 45f;     // �߻� ����

    private int _bombCount = 0;

    private float _coolTime = 0f;

    public override void Init(MiddleMonster monster)
    {
        base.Init(monster);

        _bombCount = 0;
        _coolTime = 0;
    }

    public override void Check()
    {
        if (IsCompleted) return;

        if (_coolTime >= _info.coolTime) // ��Ÿ�� Ȯ��
        {
            IsCompleted = true;
        }
        else
        {
            _coolTime += Time.deltaTime;
        }
    }

    public override void Enter()
    {
        Debug.Log("���� �̻��� �߻�");

        _coolTime = 0;

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
}
    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Dropbomb"))
        {
            _monster.Ani.SetBool("Dropbomb", true);
        }
    }

    public override void Exit()
    {
        if (_monster.Ani.GetBool("Dropbomb"))
        {
            _monster.Ani.SetBool("Dropbomb", false);
        }

        _coolTime = 0;
    }

    private void LaunchProjectile(GameObject bomb)
    {
        Vector3 targetPos = _monster.Player.transform.position + new Vector3(0, 0, 4f);
        Vector3 startPos = _monster.transform.position + new Vector3(0, 0, -1f);

        float distance = Vector3.Distance(targetPos, startPos);

        // ���� ���� ���
        float heightDifference = targetPos.y - startPos.y;

        // ���� �Ÿ��� �߻� ������ ����� �ӵ� ���
        float angleInRadians = launchAngle * Mathf.Deg2Rad;
        float horizontalDistance = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z).magnitude;

        // �߻� �ӵ� ��� (���� ���� ���)
        float velocity = Mathf.Sqrt(horizontalDistance * Physics.gravity.magnitude / Mathf.Sin(2 * angleInRadians));

        // ���� ���� ���
        Vector3 direction = (targetPos - startPos).normalized;

        // �� ���ϱ� (3D ��������)
        Vector3 velocityVector = direction * velocity * Mathf.Cos(angleInRadians);
        velocityVector.y = velocity * Mathf.Sin(angleInRadians);

        bomb.GetComponent<Rigidbody>().velocity = velocityVector;  // Rigidbody�� �ӵ� ����
    }

    private void Attack()
    {
        GameObject bomb = ObjectPool.Instance.Spawn("TraceRocket");
        bomb.transform.position = _monster.HitPoint.position + new Vector3(0, 0, -1f);

        LaunchProjectile(bomb);
    }

    private void Finish()
    {
        IsCompleted = false;
        _monster.FinishSkill();
    }
}
