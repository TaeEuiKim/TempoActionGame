using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterStat : Stat
{
    [SerializeField] private float _damageReduction;
    public float DamageReduction { get => _damageReduction; set => _damageReduction = value; }

    public override float Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                _hp = 0;
                _isDead = true;
            }
            else if (_hp > _maxHp)
            {
                _hp = _maxHp;
            }
        }
    }


    [SerializeField] private float _attackRange;
    public float AttackRange { get => _attackRange; }

    [SerializeField] private float _attackDelay;
    public float AttackDelay { get => _attackDelay; }

    private bool _isKnockBack = false;
    public bool IsKnockBack { get => _isKnockBack; set => _isKnockBack = value; }

    private bool _isStunned = false;
    public bool IsStunned { get => _isStunned; set => _isStunned = value; }

    public Action OnPointTempo = null;

    public void TakeDamage(float damage, bool isPointTempo = false)
    {
        Hp -= damage  * ((100 - _damageReduction) / 100);

        if (isPointTempo)
        {
            OnPointTempo?.Invoke();
        }
    }

}
