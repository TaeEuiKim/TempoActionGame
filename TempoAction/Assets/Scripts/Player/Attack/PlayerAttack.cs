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

    private int _attackIndex;

    private int _upgradeCount;
    public TempoCircle PointTempoCircle { get; set; }

    // 이벤트
    public bool IsHit { get; set; }
    public List<Monster> HitMonsterList { get; set; }
    public float CheckDelay { get; set; } // 체크 상태 유지 시간
    #endregion

    #region 프로퍼티

    public Define.AttackState CurrentAttackkState { get => _currentAttackState; }
    public int AttackIndex
    {
        get
        {
            return _attackIndex;
        }
        set
        {
            _attackIndex = value;

           

            _player.Ani.SetInteger("AtkCount", _attackIndex);
            
        }
    }
    public TempoAttackData CurrentAttackTempoData { get=> _player.TempoAttackDatas[_attackIndex]; }// 현재 템포 데이터 
    public int UpgradeCount
    {
        get => _upgradeCount;
        set
        {
            _upgradeCount = value;
            _upgradeCount = _upgradeCount % 4;

            _player.UpdateUpgradeCount();
        }
    }  
    #endregion

    public PlayerAttack(Player player)
    {
        _player = player;
    }

    public void Initialize()
    {
        _currentAttackState = Define.AttackState.FINISH;
        _attackStateStorage = new Dictionary<Define.AttackState, PlayerAttackState>();
        
        _attackIndex = 0;
        _upgradeCount = 0;
        PointTempoCircle = null;

        IsHit = false;
        HitMonsterList = new List<Monster>();
        CheckDelay = 0;

        //플레이어 공격 상태
        _attackStateStorage.Add(Define.AttackState.ATTACK, new AttackState(_player));
        _attackStateStorage.Add(Define.AttackState.CHECK, new CheckState(_player));
        _attackStateStorage.Add(Define.AttackState.FINISH, new FinishState(_player));

        foreach (var storage in _attackStateStorage)
        {
            storage.Value.Initialize();
        }
    }

    public void Update()
    {
        // 과부화 체크
        if (_player.CheckOverload()) 
        {
            if (_player.CurrentState == Define.PlayerState.NONE)
            {
                _player.CurrentState = Define.PlayerState.OVERLOAD;
            }
        }

        // 공격 키 입력
        if (_currentAttackState != Define.AttackState.ATTACK) 
        {
            if (Input.GetKeyDown(KeyCode.A))
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

    public bool CheckCurrentTempoType(Define.TempoType type)
    {
        if (CurrentAttackTempoData.type == type)
        {
            return true;
        }

        return false;
    }


    #region 메인 템포
    public void AttackMainTempo() // 공격 실행
    {
        if (_player.Attack.PointTempoCircle != null) return;

        if (_player.Ani.GetBool("isGrounded"))
        {
            ChangeCurrentAttackState(Define.AttackState.ATTACK);
        }
    }

    #endregion

    #region 포인트 템포

    // 포인트 템포 성공
    private void SuccessTempoCircle() 
    {
        if (_upgradeCount == 3) // 포인트 템포 업그레이트 확인
        {
            AttackIndex = 5;
            _player.Ani.SetBool("IsUpgraded", true);
        }
        else
        {
            AttackIndex = 4;
            _player.Ani.SetBool("IsUpgraded", false);
        }

        _player.Ani.SetTrigger("PointTempo");

        ChangeCurrentAttackState(Define.AttackState.ATTACK);
    }

    // 포인트 템포 실패
    private void FailureTempoCircle()
    {
        _player.Attack.PointTempoCircle = null;
        ChangeCurrentAttackState(Define.AttackState.FINISH);
    }

    // 템포 원 끝
    private void FinishTempoCircle() 
    {
        
    }

    // 템포 원 생성
    public void CreateTempoCircle(float duration = 1, Transform parent = null, Vector3 position = new Vector3()) 
    {
        SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_PointTempo_Ready", _player.transform);

        if (_player.Attack.PointTempoCircle == null)
        {
            GameObject tempoCircle = ObjectPool.Instance.Spawn("TempoCircle", 0, parent);
            tempoCircle.transform.position = new Vector3(position.x, position.y, position.z);

            _player.Attack.PointTempoCircle = tempoCircle.GetComponent<TempoCircle>();
            _player.Attack.PointTempoCircle.Init(_player.transform);           // 템포 원 초기화

            _player.Attack.PointTempoCircle.ShrinkDuration = duration;        // 탬포 원 시간 값 추가

            // 템포 이벤트 추가
            _player.Attack.PointTempoCircle.OnSuccess += SuccessTempoCircle;
            _player.Attack.PointTempoCircle.OnFailure += FailureTempoCircle;
            _player.Attack.PointTempoCircle.OnFinish += FinishTempoCircle;
        }

    }
    #endregion


    

   
}
