using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneState : IPlayerState
{
    private Player _player;

    public NoneState(Player player = null)
    {
        _player = player;
    }

    public void Enter()
    {

    }

    public void Stay()
    {
    }

    public void Exit()
    {

    }
}
