using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkAttackState : IAtkState
{
    private Player _player;


    public AtkAttackState(Player player)
    {
        _player = player;
    }

    public void Enter()
    {
      
        _player.Ani.SetBool("AttackState", true);

  
    }

    public void Exit()
    {
        _player.Ani.SetBool("AttackState", false);
        //_player.Atk.Index++;
       
    }

    public void Stay()
    {
       
    }

}
