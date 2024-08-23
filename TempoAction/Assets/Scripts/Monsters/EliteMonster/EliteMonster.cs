using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonster : Monster
{
    #region 변수

    // 상태
    private Dictionary<Define.EliteMonsterState, Elite_State> _stateStroage = new Dictionary<Define.EliteMonsterState, Elite_State>(); // 상태 저장소
    [SerializeField] private Define.EliteMonsterState _currentState = Define.EliteMonsterState.NONE;                                   // 현재 상태

    // 스킬
    [SerializeField] private List<Elite_Skill> _skillStorage = new List<Elite_Skill>();  // 스킬 저장소
    [SerializeField] private Elite_Skill _currentSkill = null;                           // 현재 스킬
    [SerializeField] private List<Elite_Skill> _readySkills = new List<Elite_Skill>();   // 준비된 스킬

    [SerializeField] private float _idleDuration;                                        // 잠시 정지 시간

    [Header("일반 공격1")]
    [SerializeField] private Transform _hitPoint;
    [SerializeField] private Vector3 _colliderSize;

    [Header("에너지볼")]
    [SerializeField] private Transform _startEnergyBallPoint; // 레이저의 시작 지점

    [Header("레이저")]
    [SerializeField] private Transform _startLaserPoint; // 레이저의 시작 지점

    [Header("돌진")]
    [SerializeField] private Vector3 _rushColliderSize;

    [Header("낙뢰")]
    [SerializeField] private CreatePlatform _createPlatform;
    #endregion;

    #region 프로퍼티

    public List<Elite_Skill> SkillStorage { get => _skillStorage; }
    public Elite_Skill CurrentSkill { get => _currentSkill; }
    public List<Elite_Skill> ReadySkills { get => _readySkills; set => _readySkills = value; }
    public float IdleDuration { get => _idleDuration; }
    public Transform HitPoint { get => _hitPoint; }
    public Vector3 ColliderSize { get => _colliderSize; }
    public Transform StartEnergyBallPoint { get => _startEnergyBallPoint; }
    public Transform StartLaserPoint { get => _startLaserPoint; }
    public Vector3 RushColliderSize { get => _rushColliderSize; }
    public CreatePlatform CreatePlatform { get => _createPlatform; }
    #endregion

    protected override void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<Player>().transform;

        _stateStroage.Add(Define.EliteMonsterState.IDLE, new Elite_Idle(this));
        _stateStroage.Add(Define.EliteMonsterState.USESKILL, new Elite_UseSkill(this));
        _stateStroage.Add(Define.EliteMonsterState.GROGGY, new Elite_Groggy(this));

        _stat.Initialize();
    }

    private void Start()
    {

        foreach (Elite_Skill s in _skillStorage)
        {
            s.Init(this);
        }

        _currentState = Define.EliteMonsterState.NONE;

    }

    private void Update()
    {
        if (_currentState != Define.EliteMonsterState.NONE)
        {
            _stateStroage[_currentState]?.Stay();
        }
    }

    public void ChangeCurrentState(Define.EliteMonsterState state)
    {
        if (_currentState != Define.EliteMonsterState.NONE)
        {
            _stateStroage[_currentState]?.Exit();
        }
        _currentState = state;
        _stateStroage[_currentState]?.Enter();
    }


    #region 스킬

    // 스킬이 끝났을 때 사용하는 함수
    public void FinishSkill()
    {
        _skillStorage.Add(_currentSkill); // 원래 저장소로 이동
        _currentSkill?.Exit();

        _currentSkill = null;

        ChangeCurrentState(Define.EliteMonsterState.IDLE);
    }

    // 현재 스킬 교체 함수
    public void ChangeCurrentSkill(Define.EliteMonsterSkill skill)
    {
        if (_currentSkill != null)
        {
            _skillStorage.Add(_currentSkill); // 원래 저장소로 이동     
            _currentSkill?.Exit();
        }

        if (skill == Define.EliteMonsterSkill.NONE)
        {
            _currentSkill = null;
        }
        else
        {
            _currentSkill = GetSkill(skill);
            _currentSkill?.Enter();
        }  
    }
    public void ChangeCurrentSkill(Elite_Skill skill)
    {
        if (_currentSkill != null)
        {
            _skillStorage.Add(_currentSkill); // 원래 저장소로 이동     
            _currentSkill?.Exit();
        }
        _currentSkill = skill;
        _currentSkill?.Enter();
    }


    // 스킬 찾는 함수
    public Elite_Skill GetSkill(Define.EliteMonsterSkill skill) 
    {
        foreach (Elite_Skill s in _skillStorage)
        {
            if (s.Info.skill == skill)
            {
                _skillStorage.Remove(s);
                return s;
            }
        }

        return null;
    }

    #endregion

   

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_hitPoint.position, _colliderSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, _rushColliderSize);
    }
}
