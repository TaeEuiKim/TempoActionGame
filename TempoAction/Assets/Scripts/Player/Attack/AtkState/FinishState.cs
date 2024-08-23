using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishState : PlayerAttackState
{
    public FinishState(Player player) : base(player)
    {

    }

    public override void Initialize()
    {

    }

    public override void Enter()
    {
        _player.Ani.SetBool("FinishState", true);

        _player.Attack.HitMonsterList.Clear(); // 공격 받은 적 리스트 비우기
        _player.Attack.AttackIndex = 0; // 공격 인덱스 초기화

    }

    public override void Stay()
    {

    }

    public override void Exit()
    {
        _player.Ani.SetBool("FinishState", false);
    }
}
