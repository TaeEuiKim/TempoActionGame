using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerAttack
{
    #region 변수
    private Player _player;

    private Define.AttackState _currentAttackState;
    private Dictionary<Define.AttackState, PlayerAttackState> _attackStateStorage;

    private Queue<TempoAttackData> _mainTempoQueue;
    private TempoAttackData _currentTempoData;

    private int _upgradeCount;
    private bool isAttack = true;

    //public TempoCircle PointTempoCircle { get; set; }

    // 이벤트
    public bool IsHit { get; set; }
    public float CheckDelay { get; set; } // 체크 상태 유지 시간
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

        _upgradeCount = 0;
        //PointTempoCircle = null;

        IsHit = false;

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

       
        if (_currentAttackState != Define.AttackState.ATTACK)
        { 
            // 공격 키 입력
            if (Input.GetKeyDown(KeyCode.X) && isAttack)
            {
                AttackMainTempo();
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
        if (_player.Ani.GetBool("isGrounded"))
        {
            CoroutineRunner.Instance.StartCoroutine(AttackTimer());

            _currentTempoData = _mainTempoQueue.Dequeue();

            ChangeCurrentAttackState(Define.AttackState.ATTACK);

            // 큐가 비어있으면 초기화
            if (_mainTempoQueue.Count == 0)
            {
                ResetMainTempoQueue();
            }
            isAttack = false;
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

    #endregion

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(0.5f);

        if (!isAttack)
        {
            isAttack = true;
        }
    }
}
