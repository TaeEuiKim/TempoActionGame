using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkFinishState : IAtkState
{

    private Player _player;

    public AtkFinishState(Player player)
    {
        _player = player;
        
    }

    public void Enter()
    {
        _player.Ani.SetBool("FinishState", true);

        _player.Atk.HitMonsterList.Clear(); // 공격 받은 적 리스트 비우기
        _player.Atk.AttackIndex = 0; // 공격 인덱스 초기화
      
    }

    public void Stay()
    {

    }

    public void Exit()
    {
        _player.Ani.SetBool("FinishState", false);
    }

}
