using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Player : CharacterBase
{
    [Header("기타")]
    private PlayerStat _playerStat;
    private PlayerView _view;
    public SkillCommand _skillCommand;

    private PlayerAttack _attack;
    private PlayerController _controller;
    private PlayerCommandController _commandController;

    [SerializeField] private Define.PlayerState _currentState = Define.PlayerState.NONE;
    private Dictionary<Define.PlayerState, PlayerState> _stateStorage = new Dictionary<Define.PlayerState, PlayerState>();

    public bool IsInvincible { get; set; } = false;

    [SerializeField] private Transform _rightSparkPoint;
    [SerializeField] private Transform _leftSparkPoint;

    [Header("움직임")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _blockLayer;

    [Header("공격")]
    [SerializeField] private Transform _hitPoint;
    [SerializeField] private Transform _endPoint;    // 넉백 지점
    [SerializeField] private Vector3 _colliderSize;
    [SerializeField] private LayerMask _monsterLayer;

    [SerializeField] private List<TempoAttackData> _mainTempoAttackDatas;
    [SerializeField] private List<TempoAttackData> _pointTempoAttackDatas;

    [Header("도깨비 불")]
    [SerializeField] private GameObject _skillObject;

    private CopySkill copySkill;

    public PlayerStat PlayerSt { get { return _playerStat; } }
    public PlayerAttack Attack { get { return _attack; } }
    public PlayerController Controller { get { return _controller; } }
    public PlayerCommandController CommandController { get { return _commandController; } }
    public Define.PlayerState CurrentState
    {
        get
        {
            return _currentState;
        }
        set
        {
            _stateStorage[_currentState]?.Exit();
            _currentState = value;
            _stateStorage[_currentState]?.Enter();
        }
    }
   
    public Transform RightSparkPoint { get => _rightSparkPoint; }
    public Transform LeftSparkPoint { get => _leftSparkPoint; }
    public Transform GroundCheckPoint { get => _groundCheckPoint; }
    public float GroundCheckRadius { get => _groundCheckRadius; }
    public LayerMask GroundLayer { get => _groundLayer; }
    public LayerMask WallLayer { get => _wallLayer; }
    public LayerMask BlockLayer { get => _blockLayer; }

    public Transform HitPoint { get => _hitPoint; }
    public Transform EndPoint { get => _endPoint; }
    public Vector3 ColliderSize { get => _colliderSize; }
    public LayerMask MonsterLayer { get => _monsterLayer; }
    public List<TempoAttackData> MainTempoAttackDatas { get => _mainTempoAttackDatas; }
    public List<TempoAttackData> PointTempoAttackDatas { get => _pointTempoAttackDatas; }
    public PlayerView View { get => _view; }

    public GameObject SkillObject { get => _skillObject; }

    [HideInInspector] public bool isTurn = false;
    [HideInInspector] public float stunTime = 0f;

    protected override void Awake()
    {
        base.Awake();

        _view = GetComponent<PlayerView>();
        _playerStat = (PlayerStat)Stat;

        copySkill = FindObjectOfType<CopySkill>();
        _attack = new PlayerAttack(this);
        _controller = new PlayerController(this);
        _commandController = new PlayerCommandController(this, _skillCommand);
    }

    private void Start()
    {
        _attack.Initialize();
        _controller.Initialize();

        //플레이어 상태
        _stateStorage.Add(Define.PlayerState.DIE, new DieState(this));
        _stateStorage.Add(Define.PlayerState.STUN, new StunState(this));
        _stateStorage.Add(Define.PlayerState.NONE, new NoneState(this));

        if (copySkill != null && copySkill.LoadSkillSlots() != null)
        {
            GetComponent<PlayerSkillManager>().LoadSkill(copySkill.LoadSkillSlots(), copySkill.LoadReserveSlots());
            _view.SetSkillIcon(copySkill.LoadMainIcon(), copySkill.LoadSubIcon());
        }
    }

    protected override void Update()
    {
        base.Update();
        if (_stat.Hp <= 0)
        {
            _currentState = Define.PlayerState.DIE;
        }

        if(!_stateStorage.ContainsKey(_currentState)) { return; }

        _stateStorage[_currentState]?.Stay();
        switch (_currentState)
        {
            case Define.PlayerState.STUN:
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                break;
            case Define.PlayerState.DIE:
                _view.OnGameoverUI();
                break;
            case Define.PlayerState.NONE:
                _attack.Update();
                _controller.Update();
                break;
        }

        if (CharacterModel.localScale.x > 0 && _skillObject.transform.localPosition.x < 0)
        {
            _skillObject.transform.DOKill();
            _skillObject.transform.DOLocalMoveX(0.68f, 0.3f);
        }
        else if (CharacterModel.localScale.x < 0 && _skillObject.transform.localPosition.x > 0)
        {
            _skillObject.transform.DOKill();
            _skillObject.transform.DOLocalMoveX(-0.68f, 0.3f);
        }
    }

    public float GetTotalDamage(bool value = true)
    {
        if (value)
        {
            return _stat.Damage + _attack.CurrentTempoData.maxDamage;
        }
        else
        {
            return _stat.Damage + _attack.CurrentTempoData.minDamage;
        }   
    }

    public override void TakeDamage(float value)
    {
        if (_playerStat.IsKnockedBack) return;

        _stat.Hp -= value * ((100 - _stat.Defense) / 100);
        UpdateHealth();
    }

    public override void TakeDamage(float value, bool isHpDamage)
    {
        if (_playerStat.IsKnockedBack || !isHpDamage) return;

        _stat.Hp -= (_stat.MaxHp * (value / 100));
        UpdateHealth();
    }

    //넉백 함수
    public void Knockback(Vector3 point, float t = 0)
    {
        transform.DOMove(point,t);
    }

    public void TakeStun(float t, int dir)
    {
        Controller.Direction = dir;
        CurrentState = Define.PlayerState.STUN;
        stunTime = t;
    }

    public void Heal(float value)
    {
        _stat.Hp += value;
        UpdateHealth();
    }

    public void PowerUp(float value)
    {
        _stat.Damage += value;
    }

    public override bool IsLeftDirection()
    {
        return CharacterModel.localScale.x > 0;
    }

    #region View
    public void UpdateHealth()
    {
        _view.UpdateHpBar(_stat.Hp / _stat.MaxHp);
    }

    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_hitPoint.position, _colliderSize);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
    }
}
