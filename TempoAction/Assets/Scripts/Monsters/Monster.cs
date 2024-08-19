using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected Rigidbody _rb;
    protected Transform _player;
    protected MonsterStat _stat;

    [SerializeField] protected LayerMask _playerLayer;
    
    protected float _direction = 1; // 몬스터가 바라보는 방향

    protected bool _canKnockback; // true =  넉백 가능한 몬스터, false = 넉백 불가능한 몬스터
    public Action OnKnockback;

    public Rigidbody Rb { get { return _rb; } }
    public Transform Player { get => _player; }
    public MonsterStat Stat { get { return _stat; } }
    public LayerMask PlayerLayer { get => _playerLayer; }
    public bool CanKnockback { get => _canKnockback; }
    public float Direction
    {
        get => _direction;
        set
        {
            if (value > 0)
            {
                value = 1;
            }
            else if (value < 0)
            {
                value = -1;
            }

            if (_direction != value)
            {
                Flip(value);
            }

            _direction = value;
        }
    }

    // 반전 함수
    public void Flip(float value) 
    {
        transform.GetChild(0).localScale = new Vector3(value, 1, 1);
    }

  

 
}
