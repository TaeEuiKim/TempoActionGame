using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Longjump", menuName = "ScriptableObjects/MiddleMonster/Skill/Longjump", order = 1)]
public class Middle_Longjump : Middle_Skill
{
    private float _coolTime = 0f;
    private bool isHit = false;
    private bool isFlying = false;

    [SerializeField] private float _knockBackPower;
    [SerializeField] private float _knockBackDuration;
    [SerializeField] float _finishDamage;                   // 피니쉬 공격 데미지

    public override void Init(MiddleMonster monster)
    {
        base.Init(monster);

        _coolTime = 0f;
        isHit = false;
        isFlying = false;
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

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
    }

    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Longjump"))
        {
            _monster.Ani.SetBool("Longjump", true);
        }

        if (!isHit && isFlying)
        {
            Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

            foreach (Collider player in hitPlayer)
            {
                if (player.GetComponent<Player>().IsInvincible) return;

                Debug.Log("멀리뛰기 성공");
                player.GetComponent<Player>().TakeDamage(_info.damage, true);
                isHit = true;
            }
        }
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("Longjump", false);
        _monster.ColliderSize = new Vector3(1, 1.5f, 1);
        _coolTime = 0;

        isHit = false;
        isFlying = false;
        IsCompleted = false;
    }

    private void Attack()
    {
        Debug.Log(_monster.Direction);
        _monster.transform.DOMoveX(_monster.Player.position.x - _monster.Direction, 1f);

        GameObject hitParticle = ObjectPool.Instance.Spawn("FX_ChungJump@P", 1); ;

        hitParticle.transform.position = new Vector3(_monster.transform.position.x, 0.6f, _monster.transform.position.z);

        isFlying = true;
    }

    private void Finish()
    {
        _monster.ColliderSize = new Vector3(_monster.ColliderSize.x * 4.5f, _monster.ColliderSize.y * 2f, _monster.ColliderSize.z);

        GameObject hitParticle = ObjectPool.Instance.Spawn("FX_ChungLanding@P", 1); ;

        hitParticle.transform.position = new Vector3(_monster.transform.position.x, 0.6f, _monster.transform.position.z);

        Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        foreach (Collider player in hitPlayer)
        {
            if (player.GetComponent<Player>().IsInvincible) return;

            Debug.Log("멀리뛰기 피니쉬 성공");
            player.GetComponent<Player>().TakeDamage(_finishDamage, true);
            player.GetComponent<Player>().TakeStun(1f);
        }

        _monster.FinishSkill();
    }
}
