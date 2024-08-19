using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObjects/Buff Data/Power Up")]
public class PowerUpBuff : BuffData
{
    [SerializeField] private float _duration;
    private float _timer = 0;

    private float _originValue; // 기존 공격력 값

    public override void Enter()
    {
        _timer = 0;

        if (_player == null)
        {
            _player = FindObjectOfType<Player>();
        }

        // 공격력 상승
        _originValue = _player.Stat.AttackDamage;
        _player.Stat.AttackDamage = value;
    }

    public override void Stay()
    {
        if (_timer >= _duration)
        {
            BuffManager.Instance.RemoveBuff(Define.BuffInfo.POWERUP); // 버프 제거
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public override void Exit()
    {
        _player.Stat.AttackDamage = _originValue; // 공격력 복구
    }
}
