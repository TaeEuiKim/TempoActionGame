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
    }

    public override void Stay()
    {
    }

    public override void Exit()
    {

    }
}
