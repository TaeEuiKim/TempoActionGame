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

    public override void Check()
    {
        if (_isCompleted) return;

        if (_coolTime >= _info.coolTime)
        {
            if (Vector2.Distance(_monster.Player.position, _monster.transform.position) <= _info.range)
            {
                _coolTime = 0;
                _isCompleted = true;
            }
        }
        else
        {
            _coolTime += Time.deltaTime;
        }
    }


    public override void Enter()
    {
        Debug.Log("멀리 치기");
        _monster.Player.GetComponent<Player>().Atk.CreateTempoCircle(_parringTime); // 포인트 템포 실행
        _tempoCircle = _monster.Player.GetComponent<Player>().Atk.PointTempoCircle;
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

        _monster.ResetSkill();
        _isCompleted = false;

    }

    public void Attack()
    {
        if (_tempoCircle.CircleState == Define.CircleState.GOOD || _tempoCircle.CircleState == Define.CircleState.PERFECT)// 패링 성공 확인
        {
            Debug.Log("패링");

            _monster.CurrentSkill = null;
            return;
        }

        bool isHit = Physics.CheckBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        if (isHit)
        {
            Debug.Log("멀리 치기 성공");
            float damage = _monster.Stat.AttackDamage * (_info.damage / 100);
            _monster.Player.GetComponent<Player>().Stat.TakeDamage(damage);
            _monster.Player.GetComponent<Player>().Stat.Knockback(Vector2.right * _monster.Direction * _knockBackPower, _knockBackDuration);
        }

        _monster.CurrentSkill = null;
    }
}
