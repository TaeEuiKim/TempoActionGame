using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Monster : MonoBehaviour
{
    private MonsterView _view;

    protected Rigidbody _rb;
    protected Transform _player;
    [SerializeField] protected MonsterStat _stat;

    [SerializeField] protected LayerMask _playerLayer;
    [SerializeField] protected LayerMask _wallLayer;
    
    protected float _direction = 1; // 몬스터가 바라보는 방향

    public Action OnKnockback;

    public bool IsGuarded { get; set; } = false;

    public Action OnPointTempo;

    [SerializeField] protected Transform _monsterModel;

    public Rigidbody Rb { get { return _rb; } }
    public Transform Player { get => _player; }
    public MonsterStat Stat { get { return _stat; } }
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
        _view = GetComponent<MonsterView>();

        Initialize();
    }

    protected abstract void Initialize();

    // 반전 함수
    public void Flip(float value) 
    {
        transform.GetChild(0).localScale = new Vector3(value, 1, 1);
    }

    public void TakeDamage(float value, bool isPointTempo = false)
    {
        if (IsGuarded)
        {
            _stat.Health -= value * ((100 - _stat.Defense) / 100);
        }
        else
        {
            _stat.Health -= value;
        }
        
        if (isPointTempo)
        {
            OnPointTempo?.Invoke();
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
