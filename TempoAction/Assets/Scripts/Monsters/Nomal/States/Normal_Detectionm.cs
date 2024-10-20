using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Detectionm : Normal_State
{
    private float _attackTimer = 0;

    public Normal_Detectionm(NormalMonster monster) : base(monster)
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

        if (_monster.MonsterSt.AttackRange < Vector3.Distance(_monster.Player.position, _monster.transform.position))
        {
            _attackTimer = 0;
        }
        else
        {

            if (_attackTimer >= _monster.MonsterSt.AttackDelay)
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
