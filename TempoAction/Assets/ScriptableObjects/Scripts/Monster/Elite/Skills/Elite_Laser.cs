using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Laser", menuName = "ScriptableObjects/EliteMonster/Skill/Laser", order = 1)]
public class Elite_Laser : Elite_Skill
{
    private float _coolTime;
    private float _totalTime;

    [SerializeField] private float _laserLength = 50.0f; // 레이저의 최대 길이
    [SerializeField] private float _laserWidth; // 레이저의 시작 지점
    //private float _laserAngle; // 레이저의 각도 (0도는 오른쪽)
    private GameObject _laser;
    public float LaserLength { get => _laserLength; }

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
        _totalTime = 0;
    }

    public override bool Check()
    {
        if (_coolTime >= _info.coolTime) // 쿨타임 확인
        {
            if (Vector2.Distance(_monster.Player.position, _monster.transform.position) <= _info.range) // 거리 확인
            {
               
                return true;
            }
        }
        else
        {
            _coolTime += Time.deltaTime;
        }

        return false;
    }

    public override void Enter()
    {
       

        Debug.Log("레이저");

        Vector2 direction = _monster.Player.position - _monster.transform.position;
        _monster.Direction = direction.x;
        //_laserAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        CoroutineRunner.Instance.StartCoroutine(StartLaser());
    }
    public override void Stay()
    {
        if (_totalTime >= _info.totalTime)
        {
            _monster.FinishSkill();
        }
        else
        {
            _totalTime += Time.deltaTime;
            
        }
    }

    public override void Exit()
    {
        _totalTime = 0;
        _coolTime = 0;
    }

    private IEnumerator StartLaser()
    {
        _monster.Ani.SetBool("Laser", true);

        yield return new WaitForSeconds(0.35f);

        _laser = ObjectPool.Instance.Spawn("Laser");
        _laser.transform.position = _monster.StartLaserPoint.position;

        Vector3 tempVec = _laser.transform.localScale;
        tempVec.x *= _monster.Direction;
        _laser.transform.localScale = tempVec;

        //_laser.transform.rotation = Quaternion.Euler(0, 0, _laserAngle);

        _laser.GetComponent<Laser>().TotalDamage = _monster.Stat.Damage * (_info.damage / 100);

        yield return new WaitForSeconds(1f);
        _laser.GetComponent<Collider>().enabled = true;

        yield return new WaitForSeconds(2f);

        _laser.GetComponent<Collider>().enabled = false;
        ObjectPool.Instance.Remove(_laser);
        _monster.Ani.SetBool("Laser", false);
    }

    /* // 레이저 발사 함수
     private void ShootLaser()
     {
         RaycastHit hit;

         // 레이저의 방향을 각도에 따라 설정 (기본 방향은 오른쪽)
         Vector3 laserDirection = Quaternion.Euler(0, 0, _laserAngle) * Vector3.right;

         Vector3 point1 = _monster.StartLaserPoint.position;
         Vector3 point2 = Vector3.zero;

         if (Physics.Raycast(_monster.StartLaserPoint.position, laserDirection, out hit, _laserLength))
         {
             // 충돌이 발생한 경우, 레이저의 끝점을 충돌 지점으로 설정
             _lineRenderer.SetPosition(0, point1);
             _lineRenderer.SetPosition(1, hit.point);

             point2 = hit.point;
         }
         else
         {
             // 충돌이 발생하지 않은 경우, 레이저의 끝점을 최대 거리로 설정
             _lineRenderer.SetPosition(0, point1);
             _lineRenderer.SetPosition(1, point1 + laserDirection * _laserLength);

             point2 = point1 + laserDirection * _laserLength;
         }

         Collider[] colliders = Physics.OverlapCapsule(point1, point2, _laserWidth / 2, _monster.PlayerLayer);

         foreach (Collider col in colliders)
         {
             Debug.Log("레이저 충돌");
             float damage = _monster.Stat.Damage * (_info.damage / 100);
             _monster.Player.GetComponent<Player>().TakeDamage(damage);
         }
     }*/
}
