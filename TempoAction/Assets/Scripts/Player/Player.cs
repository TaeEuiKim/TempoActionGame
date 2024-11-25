using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Player : CharacterBase
{
    [Header("기타")]
    private PlayerStat _playerStat;
    private PlayerView _view;
    public SkillCommand _skillCommand;

    private PlayerAttack _attack;
    private PlayerController _controller;
    private PlayerCommandController _commandController;

    private CameraController _cameraController;

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
    [SerializeField] private LayerMask _bossLayer;

    [SerializeField] private List<TempoAttackData> _mainTempoAttackDatas;
    [SerializeField] private List<TempoAttackData> _pointTempoAttackDatas;

    [Header("도깨비 불")]
    [SerializeField] private GameObject _skillObject;
    public bool isRockFireObj = false;

    [Header("궁극기")]
    [SerializeField] public GameObject[] MoveEffect;
    [SerializeField] public Material RimShader;

    [Header("공격으로 채우는 궁극기 게이지 량")]
    [SerializeField] public float fillAttackUltimateGauge = 0.02f;

    [Space]
    [Header("킬 카운트")]
    [SerializeField] public int killCount = 0;

    [HideInInspector] public bool isCounter = false;

    [Space]

    [SerializeField] public SkillRunnerBase skillData;
    [SerializeField] public SkillRunnerBase skillData2;

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
    public LayerMask BossLayer { get => _bossLayer; }
    public List<TempoAttackData> MainTempoAttackDatas { get => _mainTempoAttackDatas; }
    public List<TempoAttackData> PointTempoAttackDatas { get => _pointTempoAttackDatas; }
    public PlayerView View { get => _view; }

    public GameObject SkillObject { get => _skillObject; }

    [HideInInspector] public bool isTurn = false;
    [HideInInspector] public float stunTime = 0f;

    private bool isUseStemina = false;

    public static float saveHp = 0;
    public static float saveSte = 0;
    public static string curScene;

    protected override void Awake()
    {
        base.Awake();

        _view = FindObjectOfType<PlayerView>();
        _playerStat = (PlayerStat)Stat;

        copySkill = FindObjectOfType<CopySkill>();
        _cameraController = FindObjectOfType<CameraController>();
        _attack = new PlayerAttack(this);
        _controller = new PlayerController(this);
        _commandController = new PlayerCommandController(this, _skillCommand);
    }

    private void Start()
    {
        _attack.Initialize();
        _controller.Initialize();
        
        if (saveHp <= 0)
        {
            saveHp = PlayerSt.MaxHp;
        }
        else
        {
            PlayerSt.Hp = saveHp;
            UpdateHealth();
        }

        curScene = SceneManager.GetActiveScene().name;

        if (saveSte > 0)
        {
            View.UpdateUltimateGauge(saveSte);
        }


        //플레이어 상태
        _stateStorage.Add(Define.PlayerState.DIE, new DieState(this));
        _stateStorage.Add(Define.PlayerState.STUN, new StunState(this));
        _stateStorage.Add(Define.PlayerState.NONE, new NoneState(this));
        _stateStorage.Add(Define.PlayerState.HIT, new HitState(this));

        //if (copySkill != null && copySkill.LoadSkillSlots() != null)
        //{
        //    GetComponent<PlayerSkillManager>().LoadSkill(copySkill.LoadSkillSlots(), copySkill.LoadReserveSlots());
        //    _view.SetSkillIcon(copySkill.LoadMainIcon(), copySkill.LoadSubIcon());
        //}

        killCount = 0;

        NormalSkill sw = new NormalSkill(skillData);
        NormalSkill se = new NormalSkill(skillData2);

        ISkillRoot skill = sw;
        ISkillRoot skill2 = se;

        GetComponent<PlayerSkillManager>().AddSkill(skill);
        GetComponent<PlayerSkillManager>().AddSkill(skill2);

        RimShader.SetFloat("_Float", 0f);
    }

    protected override void Update()
    {
        base.Update();

        //Object[] os;
        //os = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        //for (int i = 0; i < os.Length; i++)
        //{
        //    Debug.LogError(os[i].name);
        //}

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
                IsInvincible = true;

                _view.OnGameoverUI();
                Ani.SetBool("IsDie", true);
                _view.UiEffect.SetActive(false);
                _cameraController.SetCameraSetting(Define.CameraType.DEAD);
                break;
            case Define.PlayerState.HIT:
                Ani.SetBool("Hit", true);
                break;
            case Define.PlayerState.NONE:
                _attack.Update();
                _controller.Update();
                break;
        }
    }

    private void LateUpdate()
    {
        if (CharacterModel.localScale.x > 0 && _skillObject.transform.localPosition.x < 0 && !isRockFireObj)
        {
            _skillObject.transform.DOKill();
            _skillObject.transform.DOLocalMoveX(0.68f, 0.3f);
        }
        else if (CharacterModel.localScale.x < 0 && _skillObject.transform.localPosition.x > 0 && !isRockFireObj)
        {
            _skillObject.transform.DOKill();
            _skillObject.transform.DOLocalMoveX(-0.68f, 0.3f);
        }
    }

    public float GetTotalDamage(bool value = true)
    {
        if (value)
        {
            return _stat.Damage;
        }
        else
        {
            return _stat.Damage;
        }   
    }

    public override void TakeDamage(float value)
    {
        if (isCounter)
        {
            Ani.SetBool("IsCounter", true);
            Ani.SetInteger("CommandCount", 22);
            Attack.ChangeCurrentAttackState(Define.AttackState.ATTACK);

            GameObject effect = ObjectPool.Instance.Spawn("energy_hit_Parrying", 1f, transform);
            GameObject effect2 = ObjectPool.Instance.Spawn("glow_view_Parrying", 1f, transform);

            effect.transform.position = transform.position + new Vector3(0, 0.7f);
            effect2.transform.position = effect.transform.position;

            isCounter = false;
            return;
        }

        if (_playerStat.IsKnockedBack) return;

        _stat.Hp -= value * ((100 - _stat.Defense) / 100);
        saveHp = _stat.Hp;
        if (CurrentState != Define.PlayerState.HIT)
        {
            CurrentState = Define.PlayerState.HIT;
        }

        UpdateHealth();
    }

    //넉백 함수
    public void Knockback(Vector3 point, float t = 0)
    {
        if (IsInvincible)
        {
            return;
        }

        transform.DOMove(point,t);
    }

    public void TakeStun(float t, int dir)
    {
        if (isCounter)
        {
            return;
        }

        Controller.Direction = -dir;
        CurrentState = Define.PlayerState.STUN;
        stunTime = t;
    }

    public void Heal(float value)
    {
        GameObject effect = ObjectPool.Instance.Spawn("FX_Heal", 1f, transform);
        effect.transform.position = transform.position + new Vector3(0, 1f, -1f);
        TestSound.Instance.PlaySound("Heal");

        if (_stat.Hp + value >= _stat.MaxHp)
        {
            _stat.Hp = _stat.MaxHp;
            saveHp = _stat.MaxHp;
        }
        else
        {
            _stat.Hp += value;
            saveHp += value;
        }
        UpdateHealth();
    }

    public void PowerUp(float value)
    {
        if (_stat.Damage + value < 2.5f)
        {
            return;
        }
        _stat.Damage = _stat.Damage + value;
    }
    
    public int SetKillCount(bool isReset = false)
    {
        if (isReset)
        {
            int temp = 0;
            temp = killCount;
            killCount = 0;
            return temp;
        }
        else
        {
            return ++killCount;
        }
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
        Gizmos.DrawWireCube(_hitPoint.position, _hitPoint.localScale);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
    }
}
