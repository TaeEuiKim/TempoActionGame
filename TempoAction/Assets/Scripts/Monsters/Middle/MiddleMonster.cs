using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleMonster : Monster
{
    [Header("상태")]
    [SerializeField] private Define.MiddleMonsterState _currentState = Define.MiddleMonsterState.NONE;                                   // 현재 상태
    private Dictionary<Define.MiddleMonsterState, Middle_State> _stateStroage = new Dictionary<Define.MiddleMonsterState, Middle_State>(); // 상태 저장소

    [Header("스킬")]
    [SerializeField] private List<Middle_Skill> _skillStorage = new List<Middle_Skill>();  // 스킬 저장소
    [SerializeField] private Middle_Skill _currentSkill = null;                           // 현재 스킬
    [SerializeField] private List<Middle_Skill> _readySkills = new List<Middle_Skill>();   // 준비된 스킬

    [Header("공격")]
    [SerializeField] private Transform _hitPoint;
    [SerializeField] private Vector3 _colliderSize;

    [Header("패링")]
    [SerializeField] private Transform _parringPoint;
    [SerializeField] private Vector3 _parringColliderSize;

    [Header("Idle 대기시간")]
    [SerializeField] private float _idleDuration;                                        // 잠시 정지 시간

    public Define.MiddleMonsterState CurrentState { get => _currentState; }

    public Middle_Skill CurrentSkill { get => _currentSkill; }

    public Action OnAttackAction;
    public Action OnFinishSkill;

    protected override void Init()
    {
        _player = FindObjectOfType<Player>().transform;

        _stateStroage.Add(Define.MiddleMonsterState.IDLE, new Middle_Idle(this));
        _stateStroage.Add(Define.MiddleMonsterState.USESKILL, new Middle_UseSkill(this));
        _stateStroage.Add(Define.MiddleMonsterState.GROGGY, new Middle_Groggy(this));
        _stateStroage.Add(Define.MiddleMonsterState.DIE, new Middle_Die(this));

        _stat.Init();
    }

    public void Enter()
    {
        foreach (Middle_Skill s in _skillStorage)
        {
            s.Init(this);
        }

        _currentState = Define.MiddleMonsterState.NONE;

        ChangeCurrentState(Define.MiddleMonsterState.IDLE);
    }

    public void Stay()
    {
        if (_currentState != Define.MiddleMonsterState.NONE)
        {
            _stateStroage[_currentState]?.Stay();
        }
    }

    public void ChangeCurrentState(Define.MiddleMonsterState state)
    {
        if (_currentState != Define.MiddleMonsterState.NONE)
        {
            _stateStroage[_currentState]?.Exit();
        }
        _currentState = state;

        if (_currentState != Define.MiddleMonsterState.NONE)
        {
            _stateStroage[_currentState]?.Enter();
        }

    }

    public void FinishSkill(Define.MiddleMonsterState state = Define.MiddleMonsterState.IDLE)
    {
        ChangeCurrentState(state);

        OnFinishSkill = null;
        OnAttackAction = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_hitPoint.position, _colliderSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_parringPoint.position, _parringColliderSize);
    }
}
