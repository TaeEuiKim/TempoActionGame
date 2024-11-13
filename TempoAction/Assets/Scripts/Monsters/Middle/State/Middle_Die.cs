using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_Die : Middle_State
{
    public Middle_Die(MiddleMonster monster) : base(monster)
    {

    }

    public override void Enter()
    {
        _monster.Ani.SetBool("Death", true);
    }

    public override void Stay()
    {
    }

    public override void Exit()
    {
    }
}
