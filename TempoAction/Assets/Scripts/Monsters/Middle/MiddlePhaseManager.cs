using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddlePhaseManager : MonoBehaviour
{
    [SerializeField] private MiddleMonster _phase1Monster;
    [SerializeField] private MiddleMonster _phase1Monster2;

    [SerializeField] private Define.MiddlePhaseState _currentPhaseState = Define.MiddlePhaseState.NONE;
    private Dictionary<Define.MiddlePhaseState, Middle_PhaseState> _phaseStateStorage = new Dictionary<Define.MiddlePhaseState, Middle_PhaseState>();

    private List<float> _targetHealthList = new List<float>();
    private int _targetHealthIndex = 0;

    public MiddleMonster Phase1Monster { get => _phase1Monster; }
    public MiddleMonster Phase1Monster2 { get => _phase1Monster2; }
    public List<float> TargetHealthList { get => _targetHealthList; }
    public int TargetHealthIndex { get => _targetHealthIndex; set => _targetHealthIndex = value; }

    private void Awake()
    {
        _phaseStateStorage.Add(Define.MiddlePhaseState.START, new Middle_PhaseStart(this));
    }

    private void Start()
    {
        //_targetHealthList.Add(_phase1Monster.Stat.MaxHp * 0.5f);
        //_targetHealthList.Add(_phase1Monster.Stat.MaxHp * 0.3f);
        //_targetHealthList.Add(_phase1Monster.Stat.MaxHp * 0.1f);
        //_targetHealthList.Add(0);
    }

    private void Update()
    {
        if (_currentPhaseState != Define.MiddlePhaseState.NONE)
        {
            _phaseStateStorage[_currentPhaseState]?.Stay();
        }
    }

    public void ChangeStageState(Define.MiddlePhaseState state)
    {
        if (_currentPhaseState != Define.MiddlePhaseState.NONE)
        {
            _phaseStateStorage[_currentPhaseState]?.Exit();
        }
        _currentPhaseState = state;
        _phaseStateStorage[_currentPhaseState]?.Enter();
    }
}
