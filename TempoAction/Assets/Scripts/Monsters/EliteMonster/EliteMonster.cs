using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonster : Monster
{
    // 상태
    private Dictionary<Define.EliteMonsterState, Elite_State> _stateStroage = new Dictionary<Define.EliteMonsterState, Elite_State>();

    [SerializeField] private Define.EliteMonsterState _currentState = Define.EliteMonsterState.NONE;
    public Define.EliteMonsterState CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState != Define.EliteMonsterState.NONE)
            {
                _stateStroage[_currentState]?.Exit();
            }           
            _currentState = value;
            _stateStroage[_currentState]?.Enter();
        }
    }

    // 스킬
    [SerializeField] private List<Elite_Skill> _phase1SkillStorage = new List<Elite_Skill>();
    public List<Elite_Skill> Phase1SkillStorage { get => _phase1SkillStorage; }

    [SerializeField] private List<Elite_Skill> _phase2SkillStorage = new List<Elite_Skill>();
    public List<Elite_Skill> Phase2SkillStorage { get => _phase2SkillStorage; }

    [SerializeField] private Elite_Skill _currentSkill = null;
    public Elite_Skill CurrentSkill
    {
        get => _currentSkill;
        set
        {
            //print("Exit :" +_currentSkill);
            _currentSkill?.Exit();     
            _currentSkill = value;

            //print("Enter :" + _currentSkill);
            _currentSkill?.Enter();
        }
    }

    [SerializeField] private List<Elite_Skill> _readySkills = new List<Elite_Skill>();
    public List<Elite_Skill> ReadySkills { get => _readySkills; set => _readySkills = value; }

    [SerializeField] private float _idleDuration;
    public float IdleDuration { get => _idleDuration; }

    [Header("일반 공격1")]
    [SerializeField] private Transform _hitPoint;
    public Transform HitPoint { get => _hitPoint; }
    [SerializeField] private Vector3 _colSize;
    public Vector3 ColSize { get => _colSize; }

    [Header("에너지볼")]
    [SerializeField] private Transform _startEnergyBallPoint; // 레이저의 시작 지점
    public Transform StartEnergyBallPoint { get => _startEnergyBallPoint; }

    [Header("레이저")]
    [SerializeField] private Transform _startLaserPoint; // 레이저의 시작 지점
    public Transform StartLaserPoint { get => _startLaserPoint; }

    [Header("돌진")]
    [SerializeField] private Vector3 _rushColSize;
    public Vector3 RushColSize { get => _rushColSize; }

    [Header("낙뢰")]
    [SerializeField] private CreatePlatform _createPlatform;
    public CreatePlatform CreatePlatform { get => _createPlatform; }
    private void Awake()
    {
        _stat = GetComponent<MonsterStat>();
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<Player>().transform;

        _stateStroage.Add(Define.EliteMonsterState.IDLE,new Elite_Idle(this));
        _stateStroage.Add(Define.EliteMonsterState.USESKILL, new Elite_UseSkill(this));
        _stateStroage.Add(Define.EliteMonsterState.GROGGY, new Elite_Groggy(this));
    }

    private void Start()
    {

        foreach (Elite_Skill s in _phase1SkillStorage)
        {
            s.Init(this);
        }

        CurrentState = Define.EliteMonsterState.IDLE;

    }

    private void Update()
    {
        if (_currentState != Define.EliteMonsterState.NONE)
        {
            _stateStroage[_currentState]?.Stay();
        }      
    }

    public void ResetSkill()
    {
        CurrentState = Define.EliteMonsterState.IDLE;
        _phase1SkillStorage.Add(_currentSkill);
    }


    public void ChangeSkill(Define.EliteMonsterSkill skill) // 스킬 교체 함수
    {       
        _currentSkill = GetSkill(skill);      
    }

    public Elite_Skill GetSkill(Define.EliteMonsterSkill skill) // 스킬 찾는 함수
    {
        foreach (Elite_Skill s in _phase1SkillStorage)
        {
            if (s.Info.skill == skill)
            {
                return s;
            }
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_hitPoint.position, _colSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, _rushColSize);
    }
}
