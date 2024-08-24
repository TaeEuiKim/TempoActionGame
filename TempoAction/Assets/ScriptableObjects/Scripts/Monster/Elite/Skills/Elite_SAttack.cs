using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SAttack", menuName = "ScriptableObjects/EliteMonster/Skill/SAttack", order = 1)]
public class Elite_SAttack : Elite_Skill
{
    private float _totalTime;

    private float _lastHealthPoints;                   // 마지막 체력 정보
    [SerializeField] private float _reductionHealthPoints; // 체력 감소량

    private TempoCircle _tempoCircle;
    [SerializeField] private float _parringDistance;
    [SerializeField] private float _parringTime;

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _totalTime = 0;
        _lastHealthPoints = _monster.Stat.MaxHealth;
    }

    public override bool Check()
    {
        if (_monster.Stat.Health + _reductionHealthPoints <= _lastHealthPoints) // 체력 감소 확인
        {
            if (Vector2.Distance(_monster.Player.position, _monster.transform.position) <= _info.range) // 거리 확인
            {
                return true;
            }
        }

        return false;
    }


    public override void Enter()
    {
        Debug.Log("일반 공격2");
        Vector3 spawnPoint = _monster.transform.position + new Vector3(_monster.Direction, 1, -1);
        _monster.Player.GetComponent<Player>().Attack.CreateTempoCircle(_parringTime, _monster.transform, spawnPoint); // 포인트 템포 실행
        _tempoCircle = _monster.Player.GetComponent<Player>().Attack.PointTempoCircle;
    }
    public override void Stay()
    {
        if (_totalTime >= _info.totalTime)
        {
            Attack();
        }
        else
        {
            _totalTime += Time.deltaTime;
            float distance = _monster.transform.position.x - _monster.Player.position.x;
            if (distance * _monster.Direction > 0 && Mathf.Abs(distance) <= _parringDistance)
            {
                _tempoCircle.IsAvailable = true;
            }
            else
            {
                _tempoCircle.IsAvailable = false;
            }
        }
    }
    public override void Exit()
    {
        _lastHealthPoints -= _reductionHealthPoints;
        _totalTime = 0;
    }

    public void Attack()
    {
        if (_tempoCircle.CircleState == Define.CircleState.GOOD || _tempoCircle.CircleState == Define.CircleState.PERFECT) // 패링 성공 확인
        {
            Debug.Log("패링");
            _monster.FinishSkill();
            return;
        }

        Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        foreach (Collider player in hitPlayer)
        {
            Debug.Log("일반 공격2 성공");
            float damage = _monster.Stat.Damage * (_info.damage / 100);
            _monster.Player.GetComponent<Player>().TakeDamage(damage);

            // 히트 파티클 생성
            GameObject hitParticle = ObjectPool.Instance.Spawn("FX_EliteAttack", 1); ;

            Vector3 hitPos = player.ClosestPoint(_monster.HitPoint.position);
            hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z - 0.1f);
        }

        _monster.FinishSkill();
    }
}
