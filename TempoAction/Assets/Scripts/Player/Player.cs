using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerStat _stat;
    public PlayerStat Stat { get { return _stat; } }

    private AtkMachine _atk;
    public AtkMachine Atk { get { return _atk; } }

    private PlayerController _controller;
    public PlayerController Controller { get { return _controller; } }

    private Rigidbody _rb;
    public Rigidbody Rb { get { return _rb; } }

    private Animator _ani;
    public Animator Ani { get { return _ani; } }

    // 플레이어 상태
    [SerializeField] private Define.PlayerState _curState = Define.PlayerState.NONE;
    public Define.PlayerState CurState
    {
        get
        {
            return _curState;
        }
        set
        {
            _stateStorage[_curState]?.Exit();
            _curState = value;
            _stateStorage[_curState]?.Enter();
        }
    }

    private Dictionary<Define.PlayerState, IPlayerState> _stateStorage = new Dictionary<Define.PlayerState, IPlayerState>();

    // 플레이어 공격 상태
    [Space]
    [SerializeField] private Define.AtkState _curAtkState = Define.AtkState.FINISH;
    public Define.AtkState CurAtkState
    {
        get
        {
            return _curAtkState;
        }
        set
        {
            _atkStateStorage[_curAtkState]?.Exit();
            _curAtkState = value;
            _atkStateStorage[_curAtkState]?.Enter();
        }
    }

    private Dictionary<Define.AtkState, IAtkState> _atkStateStorage = new Dictionary<Define.AtkState, IAtkState>();

    [SerializeField] private Transform _rightSparkPoint;
    public Transform RightSparkPoint { get => _rightSparkPoint; }

    [SerializeField] private Transform _leftSparkPoint;
    public Transform LeftSparkPoint { get => _leftSparkPoint; }

    private void Awake()
    {
        _stat = GetComponent<PlayerStat>();
        _atk = GetComponentInChildren<AtkMachine>();
        _controller = GetComponentInChildren<PlayerController>();

        _rb = GetComponent<Rigidbody>();
        _ani = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //플레이어 상태
        _stateStorage.Add(Define.PlayerState.NONE, new NoneState(this));
        _stateStorage.Add(Define.PlayerState.OVERLOAD, new OverloadState(this));
        _stateStorage.Add(Define.PlayerState.STUN, new StunState(this));

        //플레이어 공격 상태
        _atkStateStorage.Add(Define.AtkState.ATTACK, new AtkAttackState(this));
        _atkStateStorage.Add(Define.AtkState.CHECK, new AtkCheckState(this));
        _atkStateStorage.Add(Define.AtkState.FINISH, new AtkFinishState(this));
    }

    private void Update()
    {
        _stateStorage[_curState]?.Stay();

        switch (_curState)
        {
            case Define.PlayerState.STUN:
                CurAtkState = Define.AtkState.FINISH;
                break;
            case Define.PlayerState.OVERLOAD:
            case Define.PlayerState.NONE:
                _atkStateStorage[_curAtkState]?.Stay();

                _controller.Stay();

                break;
        }

    }
}
