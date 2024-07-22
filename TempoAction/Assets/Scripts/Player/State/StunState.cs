using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : IPlayerState
{
   
    private float _timer = 0;

    private PlayerManager _player;

    public StunState(PlayerManager player)
    {
        _player = player;
    }

    public void Enter()
    {
        
        _player.Ani.SetBool("IsStunned", true);
        _player.CurAtkState = Define.AtkState.FINISH;
        _timer = 0;
    }

    public void Stay()
    {
        if (_timer < _player.Stat.StunTime) // 스턴 상태일 때
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _player.CurState = Define.PlayerState.NONE;
        }
    }

    public void Exit()
    {
        _player.Ani.SetBool("IsStunned", false);
        _player.Stat.Stamina = 0;
    }

  
}
