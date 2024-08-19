using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class AtkMachine : MonoBehaviour
{
    #region 변수
    private Player _player;

    [SerializeField] private List<AtkTempoData> _atkTempoDatas = new List<AtkTempoData>();
    [SerializeField] private int _attackIndex = 0;

    private int _upgradeCount = 0;
    private TempoCircle _pointTempoCircle = null;
    #endregion

    #region 프로퍼티
    public int AttackIndex
    {
        get
        {
            return _attackIndex;
        }
        set
        {
            _attackIndex = value;

            if (_attackIndex == 4)
            {
                CreateTempoCircle();
            }

            _player.Ani.SetInteger("AtkCount", _attackIndex);
            
        }
    }
    public AtkTempoData CurAtkTempoData { get=> _atkTempoDatas[_attackIndex]; }// 현재 템포 데이터 
    public int UpgradeCount
    {
        get => _upgradeCount;
        set
        {
            _upgradeCount = value;
            _upgradeCount = _upgradeCount % 4;

            UIManager.Instance.GetUI<Slider>("UpgradeCountSlider").value = _upgradeCount;
        }
    }  
    public TempoCircle PointTempoCircle { get => _pointTempoCircle; set => _pointTempoCircle = value; }
    #endregion

    private void Start()
    {
        _player = transform.parent.GetComponent<Player>();
    }

    private void Update()
    {
        // 과부화 체크
        if (_player.Stat.CheckOverload()) 
        {
            if (_player.CurState == Define.PlayerState.NONE)
            {
                _player.CurState = Define.PlayerState.OVERLOAD;
            }
        }

        // 공격 키 입력
        if (_player.CurAtkState != Define.AtkState.ATTACK) 
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _player.Atk.AttackMainTempo();
            }
        }
    }

    #region 메인 템포
    public void AttackMainTempo() // 공격 실행
    {
        if (_pointTempoCircle != null) return;

        if (_player.Ani.GetBool("isGrounded"))
        {
            _player.CurAtkState = Define.AtkState.ATTACK;
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

        _player.CurAtkState = Define.AtkState.ATTACK;
    }

    // 포인트 템포 실패
    private void FailureTempoCircle()
    {
        _pointTempoCircle = null;
        _player.CurAtkState = Define.AtkState.FINISH;
    }

    // 템포 원 끝
    private void FinishTempoCircle() 
    {
        
    }

    // 템포 원 생성
    public void CreateTempoCircle(float duration = 1) 
    {
        SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_PointTempo_Ready", transform);

        if (_pointTempoCircle == null)
        {
            GameObject tempoCircle = ObjectPool.Instance.Spawn("TempoCircle", 0, transform.parent);
            tempoCircle.transform.position = new Vector3(transform.position.x, transform.position.y + 2, -0.1f);

            _pointTempoCircle = tempoCircle.GetComponent<TempoCircle>();
            _pointTempoCircle.Init(transform.parent);           // 템포 원 초기화

            _pointTempoCircle.ShrinkDuration = duration;        // 탬포 원 시간 값 추가

            // 템포 이벤트 추가
            _pointTempoCircle.OnSuccess += SuccessTempoCircle;
            _pointTempoCircle.OnFailure += FailureTempoCircle;
            _pointTempoCircle.OnFinish += FinishTempoCircle;
        }

    }
    #endregion


    #region 애니메이션 이벤트 함수

    private List<Monster> _hitMonsterList = new List<Monster>();
    public List<Monster> HitMonsterList { get => _hitMonsterList; set => _hitMonsterList = value; }

    [Header(("충돌"))]
    [SerializeField] private Transform _endPoint;    // 넉백 지점
    [SerializeField] private Transform _hitPoint;    // 충돌 지점

    [SerializeField] private Vector3 _colliderSize;
    [SerializeField] private LayerMask _monsterLayer;
   
    private bool _isHit = false;
    public float _checkDelay = 0; // 체크 상태 유지 시간
   
    private float _knockBackTimer = 0;

    public Transform HitPoint { get { return _hitPoint; } }
    public float CheckDelay { get => _checkDelay; set => _checkDelay = value; }
   

    // 플레이어 타격 위치에 사용하는 이벤트 함수
    private void Hit()
    {
        Collider[] hitMonsters = Physics.OverlapBox(_hitPoint.position, _colliderSize / 2, _hitPoint.rotation, _monsterLayer);

        if (hitMonsters.Length <= 0)
        {
            return;
        }

        if (_isHit == false && CurAtkTempoData.type == Define.TempoType.MAIN)
        {
            SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_RhythmCombo_Hit_" + (_attackIndex + 1), transform);
        }
        _isHit = true;

        float damage = _player.Stat.AttackDamage + CurAtkTempoData.damage;

        foreach (Collider monster in hitMonsters)
        {

            Monster m = monster.GetComponent<Monster>();


            // 데미지 입히기
            if (CurAtkTempoData.type == Define.TempoType.POINT)
            {
                m.Stat.TakeDamage(damage, true);
            }
            else
            {
                m.Stat.TakeDamage(damage);
            }

            if (m.CanKnockback)
            {
                KnockBack(m);
            }

            // 히트 파티클 생성
            GameObject hitParticle = null;
            if (CurAtkTempoData.type == Define.TempoType.POINT)
            {
         
                hitParticle = ObjectPool.Instance.Spawn("P_point_attack", 1);
            }
            else
            {
          
                hitParticle = ObjectPool.Instance.Spawn("P_main_attack", 1);
            }

            Vector3 hitPos = monster.ClosestPoint(_hitPoint.position);
            hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, -0.1f);

            HitMonsterList.Add(monster.GetComponent<Monster>());
        }

    }


    // 넉백 위치에 사용하는 이벤트 함수
    public void KnockBack(Monster monster) 
    {
        Vector2 distance = monster.transform.position - _hitPoint.position;

        Vector2 ep = (Vector2)_endPoint.position + distance;

        _knockBackTimer = 0;
        monster.Stat.IsKnockBack = true;


        monster.transform.DOMove(ep, 0.2f).OnComplete(() =>
        {
            monster.Stat.IsKnockBack = false;

            if (!monster.Stat.IsStunned)
            {
                StartCoroutine(Stun(monster));

            }

        });


    }

    // 넉백 후 잠시 스턴 
    private IEnumerator Stun(Monster monster)
    {
        monster.Stat.IsStunned = true;
        while (_knockBackTimer < 0.5f)
        {
            _knockBackTimer += Time.deltaTime;

            yield return null;
        }
        monster.Stat.IsStunned = false;

        monster.OnKnockback?.Invoke();
    }

    // 포인트 템포 애니메이션 끝에 추가하는 이벤트 함수
    private void FinishPointTempo()
    {
        float addStamina = 0;

        if (CurAtkTempoData.type == Define.TempoType.POINT) // 포인트 템포일 때
        {
            if (_isHit)
            {
                addStamina = CurAtkTempoData.maxStamina;

                if (_pointTempoCircle.CircleState == Define.CircleState.GOOD) // 타이밍이 Good일 경우
                {
                    addStamina = CurAtkTempoData.minStamina;
                }

                UpgradeCount++;

            }

        }

        _player.Stat.Stamina += addStamina;
        _isHit = false;

        _pointTempoCircle = null;
        _player.CurAtkState = Define.AtkState.FINISH;
    }

    // 메인 템포 애니메이션 끝에 추가하는 이벤트 함수
    private void Finish(float delay) 
    {

        float addStamina = 0;

        if (_isHit)
        {
            addStamina = CurAtkTempoData.maxStamina;
        }


        _player.Stat.Stamina += addStamina;
        _isHit = false;

        CheckDelay = delay;
        _player.CurAtkState = Define.AtkState.CHECK;

      
        AttackIndex++;
        

    }

    // 공격 사거리 안에 적이 있으면 적 앞으로 이동하는 이벤트 함수
    private void MoveToClosestMonster(float duration) 
    {
        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Vector3 rayDirection = transform.localScale.x > 0 ? transform.right : transform.right * -1;

        // 레이캐스트 히트 정보 저장
        RaycastHit hit;

        // 레이캐스트 실행
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, CurAtkTempoData.distance, _monsterLayer))
        {
            float closestMonsterX = hit.transform.position.x + (-rayDirection.x * 0.75f);
            transform.parent.DOMoveX(closestMonsterX, duration);
        }
 

        // 디버그용 레이 그리기
        Debug.DrawRay(rayOrigin, rayDirection * CurAtkTempoData.distance, Color.red);
    }

    // 시간 크기 변경
    private void ChangeTimeScale(float value) 
    {
        Time.timeScale = value;
    }
    // 시간 크기 복구
    private void ReturnTimeScale() 
    {
        Time.timeScale = 1;
    }
    //타임라인 실행 함수
    private void StartTimeline(string name)
    {
        TimelineManager.Instance.PlayTimeline(name);
    }

    // 중력 조절
    private void GravityActive(int value) 
    {
        foreach (Monster monster in HitMonsterList)
        {
            monster.Rb.useGravity = (value == 0) ? false : true;
        }
    }

    // 콜라이더 활성화/비활성화
    private void SetColliderActive(int value) 
    {
        _player.GetComponent<Collider>().enabled = (value == 0) ? false : true;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_hitPoint.position, _colliderSize);
    }
 
}
