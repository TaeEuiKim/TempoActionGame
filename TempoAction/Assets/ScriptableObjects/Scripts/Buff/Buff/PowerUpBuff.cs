using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObjects/Buff Data/Power Up")]
public class PowerUpBuff : BuffData
{
    public float duration;
    private float _timer = 0;

    private float _originValue;

    public override void Enter()
    {
        _timer = 0;

        if (_player == null)
        {
            _player = FindObjectOfType<Player>();
        }
        _originValue = _player.Stat.AttackDamage;
        _player.Stat.AttackDamage = value;
    }

    public override void Stay()
    {
        if (_timer >= duration)
        {
            BuffManager.Instance.RemoveBuff(Define.BuffInfo.POWERUP);
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public override void Exit()
    {
        _player.Stat.AttackDamage = _originValue;
    }
}
