using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FAttack", menuName = "ScriptableObjects/EliteMonster/Skill/FAttack", order = 1)]
public class Elite_FAttack : Elite_Skill
{
    private float _coolTime;
    private float _totalTime;

    private TempoCircle _tempoCircle;
    [SerializeField] private float _parringTime; // 패링 시간

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
        Debug.Log("일반 공격1");
        _monster.Player.GetComponent<Player>().Attack.CreateTempoCircle(_parringTime, _monster.transform, new Vector3(_monster.transform.position.x + _monster.Direction, _monster.transform.position.y + 1, -0.1f)); // 포인트 템포 실행
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
        }

    }

    public override void Exit()
    {
        _tempoCircle = null;
        _totalTime = 0;
        _coolTime = 0;
    }

    // 공격 함수
    public void Attack()
    {
        if (_tempoCircle.CircleState == Define.CircleState.GOOD || _tempoCircle.CircleState == Define.CircleState.PERFECT) // 패링 성공 확인
        {
            Debug.Log("패링");
            _monster.FinishSkill();
            return;
        }

        bool isHit = Physics.CheckBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        if (isHit)
        {
            Debug.Log("일반 공격1 성공");
            float damage = _monster.Stat.Damage * (_info.damage / 100);
            _monster.Player.GetComponent<Player>().TakeDamage(damage);
        }
        _monster.FinishSkill();
    }
}