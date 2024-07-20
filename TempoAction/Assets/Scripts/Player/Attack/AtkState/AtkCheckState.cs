using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkCheckState : IAtkState
{

    private PlayerManager _player;


    public AtkCheckState(PlayerManager player)
    {
        _player = player;
    }

    public void Enter()
    {
        if (_player.Atk.Index == 4)
        {
            _player.Atk.StartPointTempCircle();
        }
    }

    public void Exit()
    {
    }

    public void Stay()
    {

      
    }


}
