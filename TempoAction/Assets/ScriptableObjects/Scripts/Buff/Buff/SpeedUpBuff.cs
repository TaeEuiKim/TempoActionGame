using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedUp", menuName = "ScriptableObjects/Buff Data/Speed Up")]
public class SpeedUpBuff : BuffData
{
   

    public float duration;
    private float _timer = 0;

    private float _originValue;

    public override void Enter()
    {
        _timer = 0;

        if (_player == null)
        {
            _player = FindObjectOfType<PlayerManager>();
        }
        _originValue = _player.Stat.Speed;
        _player.Stat.Speed = value;
    }

    public override void Stay()
    {
        if (_timer >= duration)
        {
            BuffManager.Instance.RemoveBuff(Define.BuffInfo.SPEEDUP);
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public override void Exit()
    {
        _player.Stat.Speed = _originValue;
    }
}
