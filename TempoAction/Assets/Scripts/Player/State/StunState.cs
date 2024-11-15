using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : PlayerState
{
   
    private float _timer = 0;

    public StunState(Player player) : base(player)
    {

    }

    public override void Enter()
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
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.FINISH);
        _timer = 0;
    }

    public override void Stay()
    {
        if (_player.PlayerSt.Hp <= 0)
        {
            _player.CurrentState = Define.PlayerState.DIE;
        }

        if (_timer < _player.stunTime) // 스턴 상태일 때
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _player.CurrentState = Define.PlayerState.NONE;
        }
    }

    public override void Exit()
    {
        _player.Ani.SetBool("IsStunned", false);
        _player.Controller.isMove = true;
        _player.Controller.isJump = true;
    }

  
}
