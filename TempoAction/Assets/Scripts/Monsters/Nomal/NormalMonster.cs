using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class NormalMonster : Monster
{
    [System.Serializable]
    public class PerceptionRange
    {
        [Range(0, 100)]
        public int range_Ratio;
        public float gaugeIncrementPerSecond;
        public Color color;
    }

    #region 변수
    private INomalMonsterView _nomalMonsterView;

    [SerializeField] private float _moveRange; // Idle 이동 거리


    [Space]
    [SerializeField] private float _perceptionDistance;                                                                                    // 인식 거리
    //[SerializeField] private float _perceptionAngle;                                                                                       // 인식 각도
    //[SerializeField] private List<PerceptionRange> _perceptionRanges = new List<PerceptionRange>();                                        // 인식 범위
    private Dictionary<Define.PerceptionType, Normal_State> _perceptionStateStorage = new Dictionary<Define.PerceptionType, Normal_State>(); // 인식 상태 저장소
    [SerializeField] private Define.PerceptionType _currentPerceptionState;                                                                  // 현재 인색 상태 
    private MonsterSkillSlot[] _currentSlots;
    private Transform _target;

    /*[SerializeField] private float _maxAggroGauge; // 최대 어그로 게이지
    private float _aggroGauge = 0;                 // 현재 어그로 게이지*/

    public Vector2 SpawnPoint { get; set; }


    [Space]
    [SerializeField] private Transform _hitPoint;
    [SerializeField] private Vector3 _colliderSize;

    [Space]
    [Header("피격 대기시간")]
    [SerializeField] private float hittingTime = 0.3f;
    [Header("피격 쉐이더")]
    [SerializeField] private Material hitShader;
    [SerializeField] private Material[] originShader;
    [SerializeField] private SkinnedMeshRenderer[] skinnedMesh;

    private float _hitTimer = 0f;
    private Coroutine _hitCoroutine;
    public bool isHit = false;
    public bool isHiting = false;
    public bool hitRock = false;

    [Space]
    [Header("움직임")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 0.2f;

    [Space]
    [Header("상태 전환 조건")]
    public StateChangeConditions stateConditions;

    [Space]
    [Header("몬스터 타입")]
    public Define.NormalMonsterType monsterType;

    [Space]
    [Header("궁극기 이펙트")]
    [SerializeField] public GameObject ultimateEffect;
    [Header("궁극기 채워지는 양")]
    [SerializeField] public float ultimateValue;

    private bool isTrace = false;

    #endregion

    #region 프로퍼티
    public bool isAttack { get; set; }

    public Transform Target { get => _target; }
    public float MoveRange { get => _moveRange; }
    public Transform HitPoint { get => _hitPoint; set => _hitPoint = value; }
    public Vector3 ColliderSize { get => _colliderSize; set => _colliderSize = value; }
    public Transform GroundCheckPoint { get => _groundCheckPoint; }
    public float GroundCheckRadius { get => _groundCheckRadius; }
    public bool IsTrace { set => isTrace = value; get => isTrace; }



    public float PerceptionDistance { get => _perceptionDistance; }
    //public float PerceptionAngle { get => _perceptionAngle; }
    //public List<PerceptionRange> PerceptionRanges { get => _perceptionRanges; }
    public Define.PerceptionType PreviousPerceptionState { get; private set; }
    public Define.PerceptionType CurrentPerceptionState
    {
        get
        {
            return _currentPerceptionState;
        }
        set
        {
            // 변경 불가한 상태라면 상태를 전환하지 않음
            // 단, 아예 Conditions이 할당되지 않은 경우에는 해당 기능을 사용하지 않는 것으로 간주 -> 실행 가능
            if (stateConditions != null)
            {
                if (!stateConditions.GetChangable((int)_currentPerceptionState, (int)value)) { return; }
            }

            ForceChangeState(value);
        }
    }

    /*public float MaxAggroGauge { get => _maxAggroGauge; }
    public float AggroGauge
    {
        get => _aggroGauge;
        set
        {

            if (value < 0)
            {
                _aggroGauge = 0;
            }
            else
            {
                _aggroGauge = value;

            }

            if (_aggroGauge > _maxAggroGauge)
            {
                _aggroGauge = _maxAggroGauge;
            }

        }
    }*/
    public MonsterSkillSlot[] CurrentSkillSlots { get => _currentSlots; set => _currentSlots = value; }
   

    #endregion

    protected override void Init()
    {
        _nomalMonsterView = GetComponent<INomalMonsterView>();

        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<Player>().transform;

        _stat.Init();
    }

    private void Start()
    {
        //_perceptionStateStorage.Add(Define.PerceptionType.PATROL, new Nomal_Patrol(this));
        //_perceptionStateStorage.Add(Define.PerceptionType.BOUNDARY, new Nomal_Boundary(this));
        _perceptionStateStorage.Add(Define.PerceptionType.IDLE, new Normal_IdleState(this));
        _perceptionStateStorage.Add(Define.PerceptionType.HIT, new Normal_HitState(this));
        _perceptionStateStorage.Add(Define.PerceptionType.TRACE, new Normal_TraceState(this));
        _perceptionStateStorage.Add(Define.PerceptionType.GUARD, new Normal_GuardState(this));
        _perceptionStateStorage.Add(Define.PerceptionType.DETECTIONM, new Normal_Detectionm(this));
        _perceptionStateStorage.Add(Define.PerceptionType.SKILLATTACK, new Normal_SkillAttackState(this));
        _perceptionStateStorage.Add(Define.PerceptionType.NORMALATTACK, new Normal_NormalAttackState(this));
        _perceptionStateStorage.Add(Define.PerceptionType.DEATH, new Normal_Death(this));

        CurrentPerceptionState = Define.PerceptionType.IDLE;

        _stat.Hp = _stat.MaxHp;

        _target = CharacterManager.Instance.GetCharacter(PlayerLayer.value)[0].transform;

        // 넉백 시 실행하는 이벤트
        OnKnockback += () =>
        {
            float dir = _player.position.x - transform.position.x;
            Direction = dir;
        };

        isAttack = true;
    }

    protected override void Update()
    {
        //CheckPerceptionState(); // 게이지 증가 시키는 함수
        //UpdatePerceptionState(); // 게이지 확인 후 인식 상태 업데이트

        if (_skillManager != null)
        {
            _skillManager.OnUpdate(this);
        }

        // 인식 범위 안에 들어왔을 때
        /*if (_perceptionStateStorage[_currentPerceptionState].IsEntered)
        {
            _perceptionStateStorage[_currentPerceptionState]?.Stay();
        }*/
        _perceptionStateStorage[_currentPerceptionState]?.Stay();
    }



    #region AggroLegacy
    /*// 부채꼴 안에 플레이어가 있는지 확인
    private void CheckPerceptionState() 
    {
        #region legacy
        // 기준점에서 플레이어로 향하는 벡터 계산

        Vector3 playerPos = new Vector3(_player.position.x, _player.position.y, _player.position.z);
        Vector3 directionToPlayer = playerPos - transform.position;
        directionToPlayer.z = 0; // 높이(z축)는 고려하지 않음

        // 플레이어와의 거리 계산
        float distanceToPlayer = directionToPlayer.magnitude;
        float angleToPlayer = Vector3.Angle(new Vector3(_direction, 0, 0), directionToPlayer);

        if (CurrentPerceptionState == Define.PerceptionType.DETECTIONM)
        {
            if (distanceToPlayer > _perceptionDistance)
            {
                AggroGauge -= 20 * Time.deltaTime;
            }
        }
        else
        {
            if (distanceToPlayer > _perceptionDistance || angleToPlayer > _perceptionAngle / 2.0f) // 플레이어가 부채꼴의 반지름 내에 없을 때
            {
                AggroGauge -= 20 * Time.deltaTime;
            }
            else // 플레이어가 부채꼴의 반지름 내에 있을 때
            {
                // 게이지 증가
                if (distanceToPlayer <= _perceptionDistance * ((float)_perceptionRanges[0].range_Ratio / 100.0f))
                {
                    AggroGauge += _perceptionRanges[0].gaugeIncrementPerSecond * Time.deltaTime;
                }
                else if (distanceToPlayer <= _perceptionDistance * ((float)_perceptionRanges[1].range_Ratio / 100.0f))
                {
                    AggroGauge += _perceptionRanges[1].gaugeIncrementPerSecond * Time.deltaTime;
                }
                else if (distanceToPlayer <= _perceptionDistance * ((float)_perceptionRanges[2].range_Ratio / 100.0f))
                {
                    AggroGauge += _perceptionRanges[2].gaugeIncrementPerSecond * Time.deltaTime;
                }

            }
        }

        UpdatePerceptionGauge();
    }

    // 인식 상태 변화 업데이트 함수
    private void UpdatePerceptionState()
    {
        if (_aggroGauge == 0)
        {
            if (CurrentPerceptionState != Define.PerceptionType.IDLE)
            {
                CurrentPerceptionState = Define.PerceptionType.IDLE;
            }
        }
        else if (_aggroGauge == 10)
        {
            if (CurrentPerceptionState != Define.PerceptionType.DETECTIONM)
            {
                CurrentPerceptionState = Define.PerceptionType.DETECTIONM;
            }
        }
        else
        {
            if (CurrentPerceptionState != Define.PerceptionType.BOUNDARY)
            {
                CurrentPerceptionState = Define.PerceptionType.BOUNDARY;
            }
        }
    }*/
    #endregion

    public override void TakeDamage(float value)
    {
        base.TakeDamage(value);

        if (skinnedMesh.Length > 0)
        {
            if (_hitCoroutine != null)
            {
                StopCoroutine(_hitCoroutine);
            }

            if (monsterType == Define.NormalMonsterType.MON3)
            {
                for (int i = 0; i < skinnedMesh.Length; ++i)
                {
                    originShader[i] = skinnedMesh[i].material;
                    skinnedMesh[i].material = hitShader;
                }
            }
            else
            {
                for (int i = 0; i < skinnedMesh.Length; ++i)
                {
                    skinnedMesh[i].material = hitShader;
                }
            }

            _hitCoroutine = StartCoroutine(TurnOffShader());
        }

        if (Stat.Hp <= 0)
        {
            CurrentPerceptionState = Define.PerceptionType.DEATH;
        }
        else
        {
            if (hitRock)
            {
                return;
            }

            bool isAnim = Ani.GetCurrentAnimatorStateInfo(0).IsTag("Hit");

            if (isHit && isAnim)
            {
                Rb.velocity = Vector3.zero;
                Rb.AddForce(new Vector3(0, 2.5f), ForceMode.VelocityChange);
                isHiting = true;
                return;
            }

            if (monsterType == Define.NormalMonsterType.BALDO)
            {
                int num = Random.Range(0, 2);
                if (num == 0)
                {
                    TestSound.Instance.PlaySound("NormalMonster1_Beat1");
                }
                else if (num == 1)
                {
                    TestSound.Instance.PlaySound("NormalMonster1_Beat2");
                }
            }
            else if (monsterType == Define.NormalMonsterType.KUNG)
            {
                int num = Random.Range(0, 2);
                if (num == 0)
                {
                    TestSound.Instance.PlaySound("NormalMonster2_Beat1");
                }
                else if (num == 1)
                {
                    TestSound.Instance.PlaySound("NormalMonster2_Beat2");
                }
            }
            else if (monsterType == Define.NormalMonsterType.MON3)
            {
                int num = Random.Range(0, 2);
                if (num == 0)
                {
                    TestSound.Instance.PlaySound("NormalMonster3_Beat1");
                }
                else if (num == 1)
                {
                    TestSound.Instance.PlaySound("NormalMonster3_Beat2");
                }
            }

            if (CurrentPerceptionState != Define.PerceptionType.HIT)
            {
                isHit = true;
                CurrentPerceptionState = Define.PerceptionType.HIT;
            }
        }
        return;
    }

    private IEnumerator TurnOffShader()
    {
        yield return new WaitForSeconds(0.02f);

        if (monsterType == Define.NormalMonsterType.MON3)
        {
            for (int i = 0; i < skinnedMesh.Length; ++i)
            {
                skinnedMesh[i].material = originShader[i];
            }
        }
        else
        {
            foreach (var skin in skinnedMesh)
            {
                skin.material = originShader[0];
            }
        }

        yield return null;
    }

    // 스킬 공격 발동 가능한지 반환
    public bool GetSkillAttackUsable()
    {
        if (_SkillManager == null)
        {
            return false;
        }

        var slots = _SkillManager.GetUsableSkillSlots();
        float distance = Vector3.Distance(transform.position, Player.position);

        if (slots.Length > 0)
        {
            CurrentSkillSlots = slots;
            if (CurrentSkillSlots[0].skillRunner.skillData.SkillEffectValue * SkillData.cm2m >= distance
                && CurrentSkillSlots[0].IsUsable(_SkillManager))
            {
                return true;
            }
        }

        return false;
    }

    // 일반 공격 발동 가능한지 반환
    public bool GetNormalAttackUsable()
    {
        float distance = Vector3.Distance(transform.position, Player.position);

        if (MonsterSt.AttackRange >= distance && isAttack)
        {
            return true;
        }

        return false;
    }

    // 스킬 또는 일반 공격에 성공했는지 반환
    public bool TryAttack()
    {
        if(GetSkillAttackUsable())
        {
            ForceChangeState(Define.PerceptionType.SKILLATTACK);
            return true;
        }
        else if(GetNormalAttackUsable())
        {
            CurrentPerceptionState = Define.PerceptionType.NORMALATTACK;
            if (monsterType == Define.NormalMonsterType.MON3)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    TestSound.Instance.PlaySound("NormalMonster3_HitVoice1");
                    TestSound.Instance.PlaySound("NormalMonster3_Attack1");
                    Ani.SetInteger("AttackCount", 0);
                }
                else
                {
                    TestSound.Instance.PlaySound("NormalMonster3_HitVoice2");
                    TestSound.Instance.PlaySound("NormalMonster3_Attack2");
                    Ani.SetInteger("AttackCount", 1);
                }
            }
            return true;
        }

        return false;
    }

    public void ForceChangeState(Define.PerceptionType nextState)
    {
        // 입력된 값이 저장소에 없으면 상태를 전환하지 않음
        if (!_perceptionStateStorage.ContainsKey(nextState)) { return; }

        if (_perceptionStateStorage.ContainsKey(_currentPerceptionState))
        {
            _perceptionStateStorage[_currentPerceptionState]?.Exit();
        }
        PreviousPerceptionState = _currentPerceptionState;
        _currentPerceptionState = nextState;
        _perceptionStateStorage[_currentPerceptionState]?.Enter();
    }

    /*    #region View

        public void UpdatePerceptionGauge()
        {
            _nomalMonsterView.UpdatePerceptionGaugeImage(_aggroGauge/_maxAggroGauge);
        }

        #endregion*/

    private void OnEnable()
    {
        MonsterSt.Hp = MonsterSt.MaxHp;
        GetComponent<NomalMonsterView>().SetFullHp();
        Rb.useGravity = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void StartHitTimer()
    {
        StartCoroutine(CheckHitTimer());
    }

    private IEnumerator CheckHitTimer()
    {
        while (isHit)
        {
            _hitTimer += Time.deltaTime;
            if (_hitTimer > hittingTime)
            {
                _hitTimer = 0;
                isHit = false;
            }

            yield return null;
        }
    }

    public override bool IsLeftDirection()
    {
        if (monsterType == Define.NormalMonsterType.KUNG || monsterType == Define.NormalMonsterType.MON3)
        {
            return Direction != 1;
        }

        return Direction != -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_hitPoint.position, _colliderSize);

        if (SpawnPoint != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(SpawnPoint.x - _moveRange, transform.position.y, 0), new Vector3(SpawnPoint.x + _moveRange, transform.position.y, 0));
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheckPoint)
        {
            Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
        }
    }
}
