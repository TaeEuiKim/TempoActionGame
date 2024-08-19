using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkCheckState : IAtkState
{

    private Player _player;


    public AtkCheckState(Player player)
    {
        _player = player;

    }

    public void Enter()
    {
        _player.Ani.SetBool("CheckState", true);
    }

    public void Stay()
    {
        if (_player.Atk.CheckDelay <= 0)
        {
            _player.CurAtkState = Define.AtkState.FINISH;
        }
        else
        {
            _player.Atk.CheckDelay -= Time.deltaTime;
        }

    }
    public void Exit()
    {
        _player.Ani.SetBool("CheckState", false);
    }

}
