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

        if (_player.Attack.CurrentTempoData.type == Define.TempoType.MAIN)
        {
            _player.Ani.SetInteger("AtkCount", _player.Attack.CurrentTempoData.attackNumber);
        }
    
    }
    public override void Stay()
    {

    }
    public override void Exit()
    {
        _player.Ani.SetBool("AttackState", false);
    }
}
