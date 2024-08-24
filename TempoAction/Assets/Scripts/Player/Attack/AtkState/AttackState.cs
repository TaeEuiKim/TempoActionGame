using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerAttackState
{
    public AttackState(Player player) : base(player)
    {

    }

    public override void Initialize()
    {

    }
    public override void Enter()
    {
        _player.Ani.SetBool("AttackState", true);
    }
    public override void Stay()
    {

    }
    public override void Exit()
    {
        _player.Ani.SetBool("AttackState", false);
        _player.Attack.AttackIndex++;
        if (_player.Attack.AttackIndex == 4)
        {
            _player.Attack.CreateTempoCircle(1, _player.transform,
                _player.transform.position + new Vector3(0, 1, -1)); 
        }
    }
}
