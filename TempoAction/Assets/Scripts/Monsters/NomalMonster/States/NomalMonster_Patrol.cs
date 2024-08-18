using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalMonster_Patrol : NomalMonster_State
{
    private Vector2 _leftPoint;
    private Vector2 _rightPoint;

    private Vector2 _targetPosition;

    private bool movingRight = true;     // 이동 방향

    public NomalMonster_Patrol(NomalMonster monster) : base(monster)
    {
        movingRight = true;
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


        // Rigidbody2D를 사용하여 이동
        _monster.Rb.velocity = new Vector2(_monster.Direction * _monster.Stat.WalkSpeed, _monster.Rb.velocity.y);

        // 목표 지점에 도달하면 새로운 목표 지점 설정
        if (Mathf.Abs(_monster.transform.position.x - _targetPosition.x) <= 0.01f)
        {
            movingRight = !movingRight;
            ChangeTarget();
        }

    }
    public override void Exit()
    {
        base.Exit();
    }

    private void ChangeTarget()
    {
        if (_monster.MoveRange == 0) return;

        if (movingRight)
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
