using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAtkState
{
    public void Enter();
    public void Stay();
    public void Exit();
}
