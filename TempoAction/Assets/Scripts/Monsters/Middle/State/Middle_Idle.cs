using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_Idle : Middle_State
{
    public Middle_Idle(MiddleMonster monster) : base(monster)
    {

    }

    public override void Enter()
    {
    }

    public override void Stay()
    {
    }

    public override void Exit()
    {
    }

    // 플레이어 추적 함수
    private void Follow()
    {
        float direction = _monster.Player.transform.position.x - _monster.transform.position.x;
        _monster.Direction = direction;

        if (Mathf.Abs(direction) <= _monster.Stat.AttackRange)
        {
            _monster.Rb.velocity = new Vector2(0, _monster.Rb.velocity.y);
            //_monster.Ani.SetBool("Run", false);
        }
        else
        {
            _monster.Rb.velocity = new Vector2(_monster.Direction * _monster.Stat.SprintSpeed, _monster.Rb.velocity.y);
            //_monster.Ani.SetBool("Run", true);
        }
    }

    private void Stop()
    {
        //_monster.Ani.SetBool("Run", false);
        _monster.Rb.velocity = new Vector2(0, _monster.Rb.velocity.y);
    }
}
