using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleMonster : Monster
{
    [Header("����")]
    [SerializeField] private Define.MiddleMonsterState _currentState = Define.MiddleMonsterState.NONE;                                   // ���� ����
    private Dictionary<Define.MiddleMonsterState, Middle_State> _stateStroage = new Dictionary<Define.MiddleMonsterState, Middle_State>(); // ���� �����

    [Header("��ų")]
    [SerializeField] private List<Middle_Skill> _skillStorage = new List<Middle_Skill>();  // ��ų �����
    [SerializeField] private Middle_Skill _currentSkill = null;                           // ���� ��ų
    [SerializeField] private List<Middle_Skill> _readySkills = new List<Middle_Skill>();   // �غ�� ��ų

    [SerializeField] private float _idleDuration;                                        // ��� ���� �ð�

    public Define.MiddleMonsterState CurrentState { get => _currentState; }

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
}
