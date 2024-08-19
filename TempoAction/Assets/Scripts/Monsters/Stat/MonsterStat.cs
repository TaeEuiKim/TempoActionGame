using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterStat : Stat
{
    [SerializeField] private float _damageReduction; // 데미지 감소량
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackDelay;
    
    private bool _isKnockBack = false;
    private bool _isStunned = false;              
    
    public Action OnPointTempo = null;

    public float DamageReduction { get => _damageReduction; set => _damageReduction = value; }
    public float AttackRange { get => _attackRange; }
    public float AttackDelay { get => _attackDelay; }
    public bool IsKnockBack { get => _isKnockBack; set => _isKnockBack = value; }
    public bool IsStunned { get => _isStunned; set => _isStunned = value; }


    public void TakeDamage(float damage, bool isPointTempo = false)
    {
        HealthPoints -= damage  * ((100 - _damageReduction) / 100);

        if (isPointTempo)
        {
            OnPointTempo?.Invoke();
        }
    }

}
