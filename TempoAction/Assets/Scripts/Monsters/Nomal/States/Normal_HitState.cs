using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Normal_HitState : Normal_State
{
    private bool _isGrounded;
    private float timer = 0;

    public Normal_HitState(NormalMonster monster) : base(monster) { }

    public override void Enter()
    {
        base.Enter();

        if (_monster.Stat.Hp <= 0)
        {
            return;
        }

        // 애니 적용
        if (!_monster.Ani.GetBool("Hit"))
        {
            _monster.Ani.SetBool("Hit", true);
        }
        _monster.Ani.SetBool("Attack", false);
    }

    public override void Stay() 
    {
        if (_monster.isHiting)
        {
            if (!(_isGrounded = Physics.CheckSphere(_monster.GroundCheckPoint.position, _monster.GroundCheckRadius, _monster.GroundLayer | _monster.WallLayer))
                && timer < 1f)
            {
                timer = 0;
                _monster.Ani.SetInteger("HitCount", 1);
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    _monster.Ani.SetInteger("HitCount", 3);
                }
                else
                {
                    _monster.Ani.SetInteger("HitCount", 2);
                }
            }
        }

    }

    public override void Exit()
    {
        base.Exit();

        _monster.isHit = false;
        _monster.isHiting = false;
        timer = 0;
    }
}
