using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkFinishState : IAtkState
{

    private PlayerManager _player;

    public AtkFinishState(PlayerManager player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.Atk.Index = 0;
        _player.Ani.SetInteger("AtkCount", _player.Atk.Index);
        
    }

    public void Exit()
    {
       
    }

    public void Stay()
    {
      

    }

}
