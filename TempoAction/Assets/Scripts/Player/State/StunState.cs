using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : IPlayerState
{
   
    private float _timer = 0;

    private Player _player;

    public StunState(Player player)
    {
        _player = player;
    }

    public void Enter()
    {
        if (_player.Controller.Direction > 0)
        {
            ObjectPool.Instance.Spawn("spark_fung_main_R", 3, _player.RightSparkPoint);
        }
        else
        {
            ObjectPool.Instance.Spawn("spark_fung_main_L", 3, _player.LeftSparkPoint);
        }
        

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
