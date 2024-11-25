using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : PlayerState
{
    public DieState(Player player) : base(player)
    {
        _player = player;
    }

    public override void Enter()
    {
        _player.Ani.SetBool("IsDie", true);
        _player.View.UiEffect.SetActive(false);
        _player.Rb.velocity = Vector3.zero;
    }

    public override void Stay()
    {
    }

    public override void Exit()
    {

    }
}
