using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Buff Data/Heal")]
public class HealBuff : BuffData
{
    public override void Enter()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<Player>();
        }

        Debug.Log("Èú");
        _player.Stat.Hp += value;

        
    }

    public override void Stay()
    {
        BuffManager.Instance.RemoveBuff(Define.BuffInfo.HEAL);
    }

    public override void Exit()
    {
        Platform.Change(Define.BuffInfo.NONE);
    }
}
