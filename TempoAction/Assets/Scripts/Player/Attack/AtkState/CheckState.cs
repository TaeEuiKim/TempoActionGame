using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckState : PlayerAttackState
{
    private bool isChange = false;

    public CheckState(Player player) : base(player)
    {

    }

    public override void Initialize()
    {

    }

    public override void Enter()
    {
        isChange = false;
        if (!_player.Ani.GetBool("CheckState"))
        {
            _player.Ani.SetBool("CheckState", true);
        }
    }

    public override void Stay()
    {
        if (_player.Attack.CheckDelay <= 0)
        {
            isChange = true;
            _player.Attack.ChangeCurrentAttackState(Define.AttackState.FINISH);
        }
        else
        {
            _player.Attack.CheckDelay -= Time.deltaTime;
        }
    }
    public override void Exit()
    {
        _player.Ani.SetBool("CheckState", false);

        if (_player.Attack.CheckDelay <= 0 && !isChange)
        {
            _player.Attack.ChangeCurrentAttackState(Define.AttackState.FINISH);
        }
    }
}
