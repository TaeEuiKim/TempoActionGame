using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class PlayerAttack
{
    #region 변수
    private Player _player;

    private Define.AttackState _currentAttackState;
    private Dictionary<Define.AttackState, PlayerAttackState> _attackStateStorage;

    private Queue<TempoAttackData> _mainTempoQueue;
    private TempoAttackData _currentTempoData;

    //public TempoCircle PointTempoCircle { get; set; }

    // 이벤트
    public bool IsHit { get; set; }
    public float CheckDelay { get; set; } // 체크 상태 유지 시간
    public bool isAttack {  get; set; }
    #endregion

    #region 프로퍼티

    public Define.AttackState CurrentAttackkState { get => _currentAttackState; }
  
    public TempoAttackData CurrentTempoData { get=> _currentTempoData; }// 현재 템포 데이터 
    #endregion

    public PlayerAttack(Player player)
    {
        _player = player;
    }

    public void Initialize()
    {
        _currentAttackState = Define.AttackState.FINISH;
        _attackStateStorage = new Dictionary<Define.AttackState, PlayerAttackState>();

        _mainTempoQueue = new Queue<TempoAttackData>();
        _currentTempoData = null;

        IsHit = false;
        isAttack = true;

        CheckDelay = 0;

        //플레이어 공격 상태
        _attackStateStorage.Add(Define.AttackState.ATTACK, new AttackState(_player));
        _attackStateStorage.Add(Define.AttackState.CHECK, new CheckState(_player));
        _attackStateStorage.Add(Define.AttackState.FINISH, new FinishState(_player));

        foreach (var storage in _attackStateStorage)
        {
            storage.Value.Initialize();
        }

        ResetMainTempoQueue();
    }

    public void Update()
    {
        // 과부화 체크
        //if (_player.CheckOverload()) 
        //{
        //    if (_player.CurrentState == Define.PlayerState.NONE)
        //    {
        //        _player.CurrentState = Define.PlayerState.OVERLOAD;
        //    }
        //}

        if (isAttack || _player.Controller.isJump)
        {
            if (_currentAttackState != Define.AttackState.ATTACK && _player.Ani.GetInteger("CommandCount") == 0)
            { 
                // 공격 키 입력
                if (PlayerInputManager.Instance.attack)
                {
                    PlayerInputManager.Instance.attack = false;
                    if (_player.Ani.GetBool("isGrounded"))
                    {
                        AttackMainTempo();
                    }
                }
            }
            else
            {
                if (PlayerInputManager.Instance.attack || PlayerInputManager.Instance.keyX)
                {
                    PlayerInputManager.Instance.attack = false;
                    PlayerInputManager.Instance.keyX = false;
                }
            }
        }

        _attackStateStorage[_currentAttackState]?.Stay();
    }

    public void ChangeCurrentAttackState(Define.AttackState state)
    {
        _attackStateStorage[_currentAttackState]?.Exit();
        _currentAttackState = state;
        _attackStateStorage[_currentAttackState]?.Enter();
    }


    #region 메인 템포
    public void AttackMainTempo() // 공격 실행
    {
        if (_player.Ani.GetBool("isGrounded") && _mainTempoQueue.Count > 0)
        {
            TestSound.Instance.PlaySound("SmashSwing");
            _currentTempoData = _mainTempoQueue.Dequeue();

            ChangeCurrentAttackState(Define.AttackState.ATTACK);

            // 큐가 비어있으면 초기화
            if (_mainTempoQueue.Count == 0)
            {
                ResetMainTempoQueue();
            }
        }
    }

    public void ResetMainTempoQueue()
    {
        _mainTempoQueue.Clear();

        foreach (TempoAttackData data in _player.MainTempoAttackDatas)
        {
            _mainTempoQueue.Enqueue(data);
        }
    }

    public void SetMainTempoQueue(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _mainTempoQueue.Dequeue();
        }
    }

    #endregion
}
