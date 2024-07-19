using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneState : IPlayerState
{
    private PlayerManager _player;

    public NoneState(PlayerManager player = null)
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
