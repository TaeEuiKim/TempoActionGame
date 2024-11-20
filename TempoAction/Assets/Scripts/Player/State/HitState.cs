using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : PlayerState
{
    private float timer = 0;

    public HitState(Player player) : base(player)
    {

    }

    public override void Enter()
    {
        timer = 0;

        _player.Rb.velocity = Vector3.zero;
        _player.transform.DOKill();
        _player.Controller.isMove = false;
        _player.Controller.isJump = false;
    }

    public override void Stay()
    {
        if (timer <= _player.PlayerSt.hitTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            _player.CurrentState = Define.PlayerState.NONE;
        }
    }

    public override void Exit()
    {
        _player.Ani.SetBool("Hit", false);
        _player.Controller.isMove = true;
        _player.Controller.isJump = true;
    }
}
