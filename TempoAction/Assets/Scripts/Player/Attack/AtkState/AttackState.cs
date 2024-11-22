using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerAttackState
{
    private float timer = 0;
    private float commandTimer = 0;

    public AttackState(Player player) : base(player)
    {

    }

    public override void Initialize()
    {

    }
    public override void Enter()
    {
        timer = 0;
        commandTimer = 0;

        _player.Ani.SetBool("AttackState", true);
        _player.Ani.SetBool("IsAttack", true);

        if (_player.Attack.CurrentTempoData != null && _player.Attack.CurrentTempoData.type == Define.TempoType.MAIN)
        {
            _player.Ani.SetInteger("AtkCount", _player.Attack.CurrentTempoData.attackNumber);
        }
    }
    public override void Stay()
    {
        if (!_player.Ani.GetBool("IsCommand"))
        {
            if (timer > 0.3f)
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            commandTimer += Time.deltaTime;
            if (commandTimer > 1.7f)
            {
                if (_player.Ani.GetBool("IsAttack"))
                {
                    Debug.LogError("¹ßµ¿");
                    _player.Attack.ChangeCurrentAttackState(Define.AttackState.FINISH);
                }
            }
        }

    }
    public override void Exit()
    {
        _player.Ani.SetBool("AttackState", false);
    }
}
