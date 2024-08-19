using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nomal_Detectionm : Nomal_State
{
    private float _attackTimer = 0;

    public Nomal_Detectionm(NomalMonster monster) : base(monster)
    {

    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("¹ß°¢");
        _attackTimer = 0;

    }
    public override void Stay()
    {



        if (_monster.Stat.IsKnockBack)
        {
            _attackTimer = 0;
        }

        if (_monster.Stat.AttackRange < Vector3.Distance(_monster.Player.position, _monster.transform.position))
        {
            if (!_monster.Stat.IsKnockBack && !_monster.Stat.IsStunned)
            {
                _monster.Direction = _monster.Player.position.x - _monster.transform.position.x;
                _monster.Rb.velocity = new Vector3(_monster.Direction, 0, 0) * _monster.Stat.SprintSpeed;
            }

            _attackTimer = 0;

        }
        else
        {

            if (_attackTimer >= _monster.Stat.AttackDelay)
            {
                _monster.Attack();
                _attackTimer = 0;
            }
            else
            {
                _attackTimer += Time.deltaTime;
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
