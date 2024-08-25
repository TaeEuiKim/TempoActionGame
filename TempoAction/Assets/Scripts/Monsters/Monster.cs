using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Monster : MonoBehaviour
{
    private MonsterView _view;

    protected Animator _ani;
    protected Rigidbody _rb;
    protected Transform _player;
    [SerializeField] protected MonsterStat _stat;

    [SerializeField] protected LayerMask _playerLayer;
    [SerializeField] protected LayerMask _wallLayer;
    
    protected float _direction = 1; // 몬스터가 바라보는 방향

    public Action OnKnockback;

    public bool IsGuarded { get; set; } = false;


    [SerializeField] protected Transform _monsterModel;

    public Animator Ani { get => _ani;  }
    public Rigidbody Rb { get => _rb;  }
    public Transform Player { get => _player; }
    public MonsterStat Stat { get => _stat; set => _stat = value; }
    public LayerMask PlayerLayer { get => _playerLayer; }
    public LayerMask WallLayer { get => _wallLayer; }
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

    public Transform MonsterModel { get => _monsterModel; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _ani = GetComponentInChildren<Animator>();

        _view = GetComponent<MonsterView>();

        Initialize();
    }

    protected abstract void Initialize();

    // 반전 함수
    public void Flip(float value) 
    {
        Vector3 tempScale = transform.GetChild(0).localScale;

        if (value * tempScale.x < 0)
        {
            tempScale.x *= -1;
        }

        transform.GetChild(0).localScale = tempScale;

    }

    public void TakeDamage(float value)
    {
        if (IsGuarded)
        {
            _stat.Health -= value * ((100 - _stat.Defense) / 100);
        }
        else
        {
            _stat.Health -= value;
        }
        

        UpdateHealth();
    }


    #region View
    public void UpdateHealth()
    {
        _view.UpdateHpBar(_stat.Health / _stat.MaxHealth);
    }
    #endregion

}
