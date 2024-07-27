using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperJump", menuName = "ScriptableObjects/Buff Data/Super Jump")]
public class SuperJumpBuff : BuffData
{

    private float _originValue;

    public override void Enter()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerManager>();
        }

        _originValue = _player.Stat.JumpForce;
        _player.Stat.JumpForce = value;
    }

    public override void Stay()
    {
      
    }

    public override void Exit()
    {
        _player.Stat.JumpForce = _originValue;
    }
}
