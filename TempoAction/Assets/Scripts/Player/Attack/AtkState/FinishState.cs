using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishState : PlayerAttackState
{
    public FinishState(Player player) : base(player)
    {

    }

    public override void Initialize()
    {

    }

    public override void Enter()
    {
        _player.Ani.SetBool("FinishState", true);
        _player.Ani.SetBool("IsAttack", false);
        _player.Ani.SetBool("IsCommand", false);

        _player.Ani.SetBool("IsCounter", false);
        _player.isCounter = false;

        _player.Ani.SetInteger("CommandCount", 0);
        _player.Ani.SetInteger("AtkCount", 0);

        PlayerInputManager.Instance.ResetCommandKey();

        _player.Attack.ResetMainTempoQueue(); // 메인 템포 큐 초기화
    }

    public override void Stay()
    {

    }

    public override void Exit()
    {

        _player.Ani.SetBool("FinishState", false);
    }
}
