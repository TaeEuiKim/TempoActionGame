using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperPunch", menuName = "ScriptableObjects/EliteMonster/Skill/SuperPunch", order = 1)]
public class Elite_SuperPunch : Elite_Skill
{
    private float _coolTime;
    private float _totalTime;

    [SerializeField] private float _parringDistance;
    [SerializeField] private float _parringTime;
    [SerializeField] private float _knockBackPower;
    [SerializeField] private float _knockBackDuration;

    private TempoCircle _tempoCircle;

    private GameObject _punchReadyEffect;

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

        // 멀리 치기 이펙트 생성
        _punchReadyEffect = ObjectPool.Instance.Spawn("FX_EliteSuperPunchReady");
        _punchReadyEffect.transform.position = _monster.HitPoint.position;

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

            float distance = _monster.Player.position.x - _monster.transform.position.x;
            if (distance *_monster.Direction > 0 && Mathf.Abs(distance) <= _parringDistance)
            {
                _tempoCircle.IsAvailable = true;
            }
            else
            {
                _tempoCircle.IsAvailable = false;
            }

            if (_tempoCircle.CircleState != Define.CircleState.NONE)
            {
                _totalTime = _info.totalTime;
            }
        }

    }
    public override void Exit()
    {
        // 멀리 치기 이펙트 제거
        ObjectPool.Instance.Remove(_punchReadyEffect);

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

        GameObject punchEffect;

        if (_monster.Direction == 1)
        {
            punchEffect = ObjectPool.Instance.Spawn("FX_EliteSuperPunch_R", 1);     
        }
        else
        {
            punchEffect = ObjectPool.Instance.Spawn("FX_EliteSuperPunch_L", 1);
        }
        punchEffect.transform.position = _monster.HitPoint.position;

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
