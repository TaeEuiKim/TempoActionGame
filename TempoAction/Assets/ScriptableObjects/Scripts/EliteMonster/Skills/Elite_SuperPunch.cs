using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperPunch", menuName = "ScriptableObjects/EliteMonster/Skill/SuperPunch", order = 1)]
public class Elite_SuperPunch : Elite_Skill
{
    private float _coolTime;
    private float _totalTime;

    [SerializeField] private float _parringTime;
    [SerializeField] private float _knockBackPower;
    [SerializeField] private float _knockBackDuration;

    private TempoCircle _tempoCircle;

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
        _totalTime = 0;
    }

    public override bool Check()
    {
        if (_coolTime >= _info.coolTime)
        {
            if (Vector2.Distance(_monster.Player.position, _monster.transform.position) <= _info.range)
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
        Debug.Log("멀리 치기");
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
            if (_tempoCircle.CircleState != Define.CircleState.NONE)
            {
                _totalTime = _info.totalTime;
            }
        }

    }
    public override void Exit()
    {
        _tempoCircle = null;

        _coolTime = 0;
        _totalTime = 0;
    }
    public void Attack()
    {
        if (_tempoCircle.CircleState == Define.CircleState.GOOD || _tempoCircle.CircleState == Define.CircleState.PERFECT)// 패링 성공 확인
        {
            Debug.Log("패링");

            _monster.FinishSkill();
            return;
        }

        bool isHit = Physics.CheckBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        if (isHit)
        {
            Debug.Log("멀리 치기 성공");
            float damage = _monster.Stat.Damage * (_info.damage / 100);
            _monster.Player.GetComponent<Player>().TakeDamage(damage);
            _monster.Player.GetComponent<Player>().Knockback(Vector2.right * _monster.Direction * _knockBackPower, _knockBackDuration);
        }

        _monster.FinishSkill();
    }
}
