using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerAttack
{
    #region ����
    private Player _player;

    private Define.AttackState _currentAttackState;
    private Dictionary<Define.AttackState, PlayerAttackState> _attackStateStorage;

    private Queue<TempoAttackData> _mainTempoQueue;
    private TempoAttackData _currentTempoData;

    private int _upgradeCount;

    //public TempoCircle PointTempoCircle { get; set; }

    // �̺�Ʈ
    public bool IsHit { get; set; }
    public float CheckDelay { get; set; } // üũ ���� ���� �ð�
    #endregion

    #region ������Ƽ

    public Define.AttackState CurrentAttackkState { get => _currentAttackState; }
  
    public TempoAttackData CurrentTempoData { get=> _currentTempoData; }// ���� ���� ������ 
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

        //�÷��̾� ���� ����
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
        // ����ȭ üũ
        //if (_player.CheckOverload()) 
        //{
        //    if (_player.CurrentState == Define.PlayerState.NONE)
        //    {
        //        _player.CurrentState = Define.PlayerState.OVERLOAD;
        //    }
        //}

       
        if (_currentAttackState != Define.AttackState.ATTACK)
        { 
            // ���� Ű �Է�
            if (Input.GetKeyDown(KeyCode.X) && _player.Ani.GetBool("isGrounded"))
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


    #region ���� ����
    public void AttackMainTempo() // ���� ����
    {
        if (_player.Ani.GetBool("isGrounded"))
        {
            _player.Controller.isMove = false;

            _currentTempoData = _mainTempoQueue.Dequeue();

            ChangeCurrentAttackState(Define.AttackState.ATTACK);

            // ť�� ��������� �ʱ�ȭ
            if (_mainTempoQueue.Count == 0)
            {
                ResetMainTempoQueue();
            }
            //isAttack = false;
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
}
