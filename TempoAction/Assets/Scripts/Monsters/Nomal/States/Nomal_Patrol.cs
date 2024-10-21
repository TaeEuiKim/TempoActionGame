using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nomal_Patrol : Normal_State
{
    private Vector2 _leftPoint;
    private Vector2 _rightPoint;

    private Vector2 _targetPosition;

    private bool _movingRight = true;     // �̵� ����

    public Nomal_Patrol(NormalMonster monster) : base(monster)
    {
        _movingRight = true;
    }

    public override void Enter()
    {
        base.Enter();
        _monster.SpawnPoint = _monster.transform.position;

        _leftPoint = new Vector2(_monster.SpawnPoint.x - _monster.MoveRange, _monster.SpawnPoint.y);
        _rightPoint = new Vector2(_monster.SpawnPoint.x + _monster.MoveRange, _monster.SpawnPoint.y);

        ChangeTarget();
    }

    public override void Stay()
    {
        // Rigidbody2D�� ����Ͽ� �̵�
        _monster.Rb.velocity = new Vector2(_monster.Direction * _monster.Stat.WalkSpeed, _monster.Rb.velocity.y);

        // ��ǥ ������ �����ϸ� ���ο� ��ǥ ���� ����
        if (Mathf.Abs(_monster.transform.position.x - _targetPosition.x) <= 0.01f)
        {
            _movingRight = !_movingRight;
            ChangeTarget();
        }

    }
    public override void Exit()
    {
        base.Exit();
    }

    // Idle ������ �� �¿� Ÿ�� ���� �Լ�
    private void ChangeTarget()
    {
        if (_monster.MoveRange == 0) return;

        if (_movingRight)
        {
            _targetPosition = _rightPoint;
            _monster.Direction = _rightPoint.x - _monster.transform.position.x;
        }
        else
        {
            _targetPosition = _leftPoint;
            _monster.Direction = _leftPoint.x - _monster.transform.position.x;
        }
    }
}
