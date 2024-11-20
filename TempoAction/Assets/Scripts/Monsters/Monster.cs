using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : CharacterBase
{
    [field:SerializeField] public MonsterSkillManager _SkillManager { get; protected set; }
    [SerializeField] protected LayerMask _playerLayer;
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected LayerMask _wallLayer;
    [SerializeField] public SkillRunnerBase skillData;
    protected MonsterStat _monsterStat;

    protected Transform _player;
    private MonsterView _view;

    private List<float> hpList;

    protected float _direction = 1; // 몬스터가 바라보는 방향

    public Action OnKnockback;

    public bool IsGuarded { get; set; } = false;
    public bool IsParrying { get; set; } = false;


    #region 프로퍼티
    public Transform Player { get => _player; }
    public MonsterStat MonsterSt { get => _monsterStat; set => _monsterStat = value; }
    public LayerMask PlayerLayer { get => _playerLayer; }
    public LayerMask WallLayer { get => _wallLayer; }
    public LayerMask GroundLayer { get => _groundLayer; }
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
    #endregion

    protected override void Awake()
    {
        base.Awake();

        _SkillManager = GetComponent<MonsterSkillManager>();

        _view = GetComponent<MonsterView>();

        string json = JsonUtility.ToJson(Stat);
        MonsterStat _temp = ScriptableObject.CreateInstance<MonsterStat>();
        JsonUtility.FromJsonOverwrite(json, _temp);

        Stat = _temp;
        MonsterSt = _temp;

        Init();
    }

    protected abstract void Init();

    // 반전 함수
    public void Flip(float value) 
    {
        Vector3 tempScale = _characterModel.localScale;

        if (value * tempScale.x < 0)
        {
            tempScale.x *= -1;
        }

        _characterModel.localScale = tempScale;
    }

    public override void TakeDamage(float value)
    {
        if (IsGuarded)
        {
            _stat.Hp -= value * ((100 - _stat.Defense) / 100);
        }
        else
        {
            _stat.Hp -= value;
        }
        
        UpdateHealth();
    }

    public override bool IsLeftDirection()
    {
        return Direction != -1;
    }
    #region View
    public void UpdateHealth()
    {
        _view.UpdateHpBar(_stat.Hp / _stat.MaxHp);
    }
    #endregion

}
