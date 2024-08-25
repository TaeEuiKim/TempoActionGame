using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SAttack", menuName = "ScriptableObjects/EliteMonster/Skill/SAttack", order = 1)]
public class Elite_SAttack : Elite_Skill
{

    private float _coolTime;

    private TempoCircle _tempoCircle;
    [SerializeField] private float _parringTime; // 패링 시간

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
    }

    public override void Check()
    {
        if (IsCompleted) return;

        if (_coolTime >= _info.coolTime) // 쿨타임 확인
        {
            if (Vector2.Distance(_monster.Player.position, _monster.transform.position) <= _info.range) // 거리 확인
            {

                IsCompleted = true;
            }
        }
        else
        {
            _coolTime += Time.deltaTime;
        }

    }

    public override void Enter()
    {


        Debug.Log("일반 공격2");
        Vector3 spawnPoint = _monster.transform.position + new Vector3(_monster.Direction, 1, -1);
        _monster.Player.GetComponent<Player>().Attack.CreateTempoCircle(_parringTime, _monster.transform, spawnPoint); // 포인트 템포 실행
        _tempoCircle = _monster.Player.GetComponent<Player>().Attack.PointTempoCircle;

        _monster.OnHitAction += Attack;
        _monster.OnFinishSkill += Finish;
    }
    public override void Stay()
    {
        if (_tempoCircle.CircleState != Define.CircleState.NONE)
        {
            if (_monster.Ani.GetBool("SAttack")) return;

            if (Parring())
            {
                Finish();
                return;
            }

            _monster.Ani.SetBool("SAttack", true);
        }
        else
        {
            if (CheckParringBox())
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
        _tempoCircle = null;
        _coolTime = 0;
    }

    private bool CheckParringBox()
    {
        return Physics.CheckBox(_monster.ParringPoint.position, _monster.ParringColliderSize / 2, _monster.ParringPoint.rotation, _monster.PlayerLayer);
    }

    private bool Parring()
    {
        if (_tempoCircle.CircleState == Define.CircleState.GOOD || _tempoCircle.CircleState == Define.CircleState.PERFECT) // 패링 성공 확인
        {
            Debug.Log("패링");
            return true;
        }

        return false;
    }

    // 공격 함수
    private void Attack()
    {

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

    }

    private void Finish()
    {
        _monster.Ani.SetBool("SAttack", false);
        _monster.FinishSkill();
    }
}
