using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalMonster_Boundary : NomalMonster_State
{
    public NomalMonster_Boundary(NomalMonster monster) : base(monster)
    {

    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("°æ°è");
    }
    public override void Stay()
    {

    }
    public override void Exit()
    {
        base.Exit();
    }
}
