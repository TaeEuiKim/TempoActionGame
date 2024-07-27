using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TickDamage", menuName = "ScriptableObjects/Buff Data/Tick Damage")]
public class TickDamageBuff : BuffData
{
    public float giveTime;
    private float _timer = 0;

    private float _originValue;

    public override void Enter()
    {
        _timer = 0;

        if (_player == null)
        {
            _player = FindObjectOfType<PlayerManager>();
        }

    }

    public override void Stay()
    {
        if (_timer >= giveTime)
        {
            _player.Stat.TakeDamage(value);
            _timer = 0;
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    public override void Exit()
    {

    }

}
