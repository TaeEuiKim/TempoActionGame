using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_IdleState : Normal_State
{
    public Normal_IdleState(NomalMonster monster) : base(monster) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Stay()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        base.Exit();
    }

}
