using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffData : ScriptableObject
{
    protected PlayerManager _player;

    public Define.BuffType type;
    public Define.BuffInfo info;

    public float value;

    public bool IsFinished { get; set; } = false;

    public abstract void Enter();
    public abstract void Stay();
    public abstract void Exit();
}
