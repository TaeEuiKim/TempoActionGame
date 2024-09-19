using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_Longjump : Middle_Skill
{
    private float _coolTime = 0f;

    [SerializeField] private float _knockBackPower;
    [SerializeField] private float _knockBackDuration;
    [SerializeField] float _finishDamage;                   // 피니쉬 공격 데미지

    public override void Init(MiddleMonster monster)
    {
        base.Init(monster);

        _coolTime = 0f;
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
        Debug.Log("멀리뛰기");
        _monster.ColliderSize = new Vector3(_monster.ColliderSize.x * 2f, _monster.ColliderSize.y * 3f, _monster.ColliderSize.z);

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
    }

    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Longjump"))
        {
            _monster.Ani.SetBool("Longjump", true);
        }
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("Longjump", false);
        _monster.ColliderSize = new Vector3(1, 1.5f, 1);
        _coolTime = 0;

        IsCompleted = false;
    }

    private void Attack()
    {
        Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _monster.ColliderSize, _monster.HitPoint.rotation, _monster.PlayerLayer);

        foreach (Collider player in hitPlayer)
        {
            if (player.GetComponent<Player>().IsInvincible) return;

            Debug.Log("멀리뛰기 성공");
            _monster.Player.GetComponent<Player>().TakeDamage(_info.damage, true);

            // 히트 파티클 생성
            //GameObject hitParticle = ObjectPool.Instance.Spawn("FX_EliteAttack", 1); ;

            //Vector3 hitPos = player.ClosestPoint(_monster.HitPoint.position);
            //hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z - 0.1f);
        }
    }

    private Vector3 GetKnockBackPosition()
    {
        RaycastHit hit;

        if (Physics.Raycast(_monster.transform.position, Vector2.right * _monster.Direction, out hit, _knockBackPower * _knockBackDuration, _monster.WallLayer))
        {
            return hit.point;
        }

        return (Vector2.right * _monster.Direction) * (_knockBackPower * _knockBackDuration);
    }

    private void Finish()
    {
        _monster.FinishSkill();
    }
}
