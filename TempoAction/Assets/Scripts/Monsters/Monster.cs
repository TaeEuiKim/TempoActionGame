using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected Rigidbody _rb;
    public Rigidbody Rb { get { return _rb; } }

    protected Transform _player;
    public Transform Player { get => _player; }

    [SerializeField] protected LayerMask _playerLayer;
    public LayerMask PlayerLayer { get => _playerLayer; }

    protected MonsterStat _stat;
    public MonsterStat Stat { get { return _stat; } }

    protected bool _canKnockback;
    public bool CanKnockback { get => _canKnockback; }

    public Action OnKnockback;

    protected float _direction = 1;
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

    public void Flip(float value)
    {

        transform.GetChild(0).localScale = new Vector3(value, 1, 1);
    }

  

 
}
