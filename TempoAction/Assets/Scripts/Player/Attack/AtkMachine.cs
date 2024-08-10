using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class AtkMachine : MonoBehaviour
{
    private PlayerManager _player;

    [SerializeField] private List<AtkTempoData> _atkTempoDatas = new List<AtkTempoData>();

    [SerializeField] private int _index = 0;
    public int Index
    {
        get
        {
            return _index;
        }
        set
        {
            _index = value;
            _index = _index % 5;
            _player.Ani.SetInteger("AtkCount", _player.Atk.Index);

        }
    }


    public AtkTempoData CurAtkTempoData
    {
        get
        {
            if (_index == 4 && _player.Atk.UpgradeCount == 3) // 포인트 템포 강화 확인
            {
                return _atkTempoDatas[5];
            }
            else
            {
                return _atkTempoDatas[_index];
            }
        }

    }

    public bool IsUpgraded { get; set; } = false;

    private int _upgradeCount = 0;
    public int UpgradeCount
    {
        get
        {
            return _upgradeCount;
        }
        set
        {
            _upgradeCount = value;
            _upgradeCount = _upgradeCount % 4;

            UIManager.Instance.GetUI<Slider>("UpgradeCountSlider").value = _upgradeCount;


        }
    }

    [SerializeField] private TempoCircle _pointTempoCircle;
    public TempoCircle PointTempoCircle { get { return _pointTempoCircle; } }
    public Define.CircleState CircleState { get; set; } = Define.CircleState.GOOD;

    private void Start()
    {


        _player = transform.parent.GetComponent<PlayerManager>();

    }

    private void Update()
    {
        if (_player.Stat.CheckOverload()) // 과부화 체크
        {
            //Debug.Log("과부화");
            if (_player.CurState == Define.PlayerState.NONE)
            {
                _player.CurState = Define.PlayerState.OVERLOAD;
            }
        }



    }

    public bool InputAtk()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            return true;
        }

        return false;
    }

    public void Execute() // 공격 실행
    {
        if (_player.Ani.GetBool("isGrounded"))
        {
            _player.CurAtkState = Define.AtkState.ATTACK;
        }
    }


    public void StartPointTempCircle() // 포인트 템포 원 생성
    {
        if (CurAtkTempoData.type != Define.TempoType.POINT)
        {
            return;
        }
        //SoundManager.Instance.PlaySFX("SFX_PointTempo_Ready");
        _pointTempoCircle.gameObject.SetActive(true);
        _pointTempoCircle.ResetCircle();
    }

    #region 애니메이션 이벤트 함수

    public List<Enemy> HitEnemyList { get; set; } = new List<Enemy>();

    [Header(("충돌"))]
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform _hitPoint;

    [SerializeField] private Vector3 _colSize;
    [SerializeField] private LayerMask _enemyLayer;
    public Transform HitPoint { get { return _hitPoint; } }

    private bool _isHit = false;
    public float CheckDelay { get; set; } = 0;
    private void Hit()
    {
        //Collider[] hitEnemies = Physics.OverlapSphere(_hitPoint.position, _attackRadius, _enemyLayer);
        Collider[] hitEnemies = Physics.OverlapBox(_hitPoint.position, _colSize, _hitPoint.rotation, _enemyLayer);

        if (hitEnemies.Length <= 0)
        {
            return;
        }

        //SoundManager.Instance.PlaySFX("SFX_RhythmCombo_Hit1");

        _isHit = true;

        float damage = _player.Stat.Damage + CurAtkTempoData.damage;

        foreach (Collider enemy in hitEnemies)
        {

            // 데미지 입히기
            enemy.GetComponent<Enemy>().Stat.TakeDamage(damage);

            // 히트 파티클 생성
            GameObject hitParticle = null;
            if (CurAtkTempoData.type == Define.TempoType.POINT)
            {
                hitParticle = EffectManager.Instance.Pool.Spawn("P_point_attack", 1);
            }
            else
            {
                hitParticle = EffectManager.Instance.Pool.Spawn("P_main_attack", 1);
            }

            Vector3 hitPos = enemy.ClosestPoint(_hitPoint.position);
            hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, -0.1f);

            KnockBack(enemy.transform);
            HitEnemyList.Add(enemy.GetComponent<Enemy>());
        }

    }

    private void KnockBack(Transform target) // 넉백
    {
        Vector2 distance = target.position - _hitPoint.position;

        Vector2 endPoint = (Vector2)_endPoint.position + distance;

        target.DOMove(endPoint, 0.2f);
    }

    private void GravityActive(int value) // 중력 조절
    {
        foreach (Enemy enemy in HitEnemyList)
        {
            enemy.GetComponent<Rigidbody>().useGravity = (value == 0) ? false : true;
        }
    }

    private void Finish(float delay) // 공격이 끝난 시점 
    {
        if (_isHit)
        {
            float addStamina = CurAtkTempoData.maxStamina;

            if (CurAtkTempoData.type == Define.TempoType.POINT) // 포인트 템포일 때
            {
                if (CircleState == Define.CircleState.GOOD) // 타이밍이 Good일 경우
                {
                    addStamina = CurAtkTempoData.minStamina;
                }
                
                UpgradeCount++;
            }

            _player.Stat.Stamina += addStamina;
            _isHit = false;
        }

        CheckDelay = delay;
        _player.CurAtkState = Define.AtkState.CHECK;

    }

    private void MoveToClosestEnemy(float duration) // 특정 거리에 적이 있으면 적 앞으로 이동
    {
        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y+0.25f, transform.parent.position.z);
        Vector3 rayDirection = transform.localScale.x > 0 ? transform.right : transform.right * -1;

        // 레이캐스트 히트 정보 저장
        RaycastHit hit;

        // 레이캐스트 실행
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, CurAtkTempoData.distance, _enemyLayer))
        {
            Transform closestEnemy = hit.collider.GetComponent<Enemy>().moveToPoint;
            transform.parent.DOMoveX(closestEnemy.position.x, duration);
        }
 

        // 디버그용 레이 그리기
        Debug.DrawRay(rayOrigin, rayDirection * CurAtkTempoData.distance, Color.red);
    }


    private void ChangeTimeScale(float value) // 시간 크기 변경
    {
        Time.timeScale = value;
    }
    private void ReturnTimeScale() // 시간 크기 복구
    {
        Time.timeScale = 1;
    }

    private void StartTimeline(string name) //타임라인 실행 함수
    {
        TimelineManager.Instance.PlayTimeline(name);
    }

  


    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_hitPoint.position, _colSize);
    }
 
}
