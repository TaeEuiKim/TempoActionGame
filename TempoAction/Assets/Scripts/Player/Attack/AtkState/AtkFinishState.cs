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
        _player.Ani.SetBool("FinishState", true);

        //Debug.Log("³¡");
        _player.Atk.HitEnemyList.Clear();
        _player.Atk.Index = 0;
      
    }

    public void Exit()
    {
        _player.Ani.SetBool("FinishState", false);
    }

    public void Stay()
    {

        if (Input.GetKeyDown(InputManager.Instance.FindKeyCode("MainTempo")))
        {
            _player.Atk.Execute();
        }


    }

}
