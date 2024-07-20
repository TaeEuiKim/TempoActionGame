using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class AtkMachine : MonoBehaviour
{

    private ObjectPool _pool;

    private PlayerManager _player;

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


        }
    }

    private AtkTempoData _curAtkTempoData;
    public AtkTempoData CurAtkTempoData
    {
        get
        {
            return _curAtkTempoData;
        }
        set
        {
            _curAtkTempoData = value;
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


    [SerializeField] private GameObject _damageTextPrefab;

    [SerializeField] private LayerMask _enemyLayer;

    private List<Enemy> _hitEnemies = new List<Enemy>();

    [SerializeField] private Transform _endPoint;

    [SerializeField] private Transform _hitPoint;
    public Transform HitPoint { get { return _hitPoint; } }

    private bool _isHit = false;

    private float _attackRadius;

    private void Start()
    {
        _pool = FindObjectOfType<ObjectPool>();

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
     /*   if (_player.Controller.IsGrounded) // 땅에 있을 때
        {

        }
        else // 공중 일 때
        {

        }*/
       
        _player.CurAtkState = Define.AtkState.ATTACK;
        
    }
   
 
    public void StartPointTempCircle() // 포인트 템포 원 생성
    {
        _pointTempoCircle.gameObject.SetActive(true);
        _pointTempoCircle.ResetCircle();
    }

    #region 애니메이션 이벤트 함수

    private void Hit(float attackRadius)
    {
        _attackRadius = attackRadius;
        Collider[] hitEnemies = Physics.OverlapSphere(_hitPoint.position, _attackRadius, _enemyLayer);

        if (hitEnemies.Length <= 0)
        {
            return;
        }

        _isHit = true;

        float damage = _player.Stat.Damage + CurAtkTempoData.damage;

        foreach (Collider enemy in hitEnemies)
        {       
            if (enemy.transform.localPosition.z < 0)
            {
                enemy.transform.DOLocalMoveZ(0f, 0.1f);
            }

            // 데미지 입히기
            enemy.GetComponent<Enemy>().Stat.TakeDamage(damage);

            /*  // 데미지 텍스트 생성
              GameObject damageText = _pool.GetObject("DamageTextPrefab");
              damageText.transform.position = enemy.transform.position;
              damageText.GetComponent<DamageText>().DamageValue = damage;*/

            // 히트 파티클 생성
            GameObject hitParticle = null;
            if (_index == 4)
            {
                hitParticle = _pool.GetObject("P_point_attack");              
            }
            else
            {
                hitParticle = _pool.GetObject("P_main_attack");
            }

            Vector3 hitPos = enemy.ClosestPoint(_hitPoint.position);
            hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, -2);

            KnockBack(enemy.transform);
        }

        if (hitEnemies.Length > 0)
        {
            Impact();
        }
        
    }

    private void Check(float delay = 1) // 공격이 끝난 시점 
    {
        if (_isHit)
        {
            float addStamina = CurAtkTempoData.maxStamina;

            if (CurAtkTempoData.type == Define.TempoType.POINT)
            {
                if (CircleState == Define.CircleState.GOOD)
                {
                    addStamina = CurAtkTempoData.minStamina;
                }
            }

            _player.Stat.Stamina += addStamina;
            _isHit = false;
        }

        _player.CurAtkState = Define.AtkState.CHECK;

        Invoke("Finish", delay); // 끝 확인
    }

    private void Finish()
    {
        if (_player.CurAtkState == Define.AtkState.CHECK)
        {
            _player.CurAtkState = Define.AtkState.FINISH;
        }
    }

    private void DoAtkMove(float force)
    {
        transform.parent.DOMoveX(transform.parent.position.x + force * _player.Controller.DashDirection, 0.1f);
    }
    private void Impact() // 공격시 임펙트
    {
        StartCoroutine(ChangeAnimatorSpeed(0.2f, 0.2f));
    }

    private IEnumerator ChangeAnimatorSpeed(float speed, float duration) // 애니메이터 스피드 조절
    {
        float last = _player.Ani.speed;

        _player.Ani.speed = speed;
        yield return new WaitForSeconds(duration);
        _player.Ani.speed = last;
    }

    private void KnockBack(Transform target)
    {
        Vector2 distance = target.position - _hitPoint.position;

        Vector2 endPoint = (Vector2)_endPoint.position + distance;

        target.DOMove(endPoint, 0.2f);
    }
  
    

    private void GravityActive(int value)
    {
        foreach(Enemy enemy in _hitEnemies)
        {
            enemy.GetComponent<Rigidbody>().useGravity = (value == 0) ? false : true;
        }      
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_hitPoint.position, _attackRadius);
    }
  
}
