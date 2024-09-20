using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Takedown", menuName = "ScriptableObjects/MiddleMonster/Skill/Takedown", order = 1)]
public class Middle_Takedown : Middle_Skill
{
    private float _coolTime = 0f;
    private float count = 0f;
    private bool _isHit = false;

    [SerializeField] int _attackCount;                      // 공격 횟수
    [SerializeField] private float _knockBackPower;         
    [SerializeField] private float _knockBackDuration;
    [SerializeField] float _finishDamage;                   // 피니쉬 공격 데미지

    public override void Init(MiddleMonster monster)
    {
        base.Init(monster);

        _coolTime = 0f;
        count = 0f;
        _isHit = false;
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
        Debug.Log("내려찍기");
        _monster.ColliderSize = new Vector3(_monster.ColliderSize.x * 2f, _monster.ColliderSize.y * 3f, _monster.ColliderSize.z);

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
    }

    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Takedown"))
        {
            _monster.Ani.SetBool("Takedown", true);
        }
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("Takedown", false);
        _monster.ColliderSize = new Vector3(1, 1.5f, 1);
        _coolTime = 0;
        count = 0f;
        _isHit = false;

        IsCompleted = false;
    }

    private void Attack()
    {
        Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        foreach (Collider player in hitPlayer)
        {
            if (player.GetComponent<Player>().IsInvincible) return;

            if (count < _attackCount)
            {
                Debug.Log("내려찍기 성공");
                player.GetComponent<Player>().TakeDamage(_info.damage, true);
            }
            else
            {
                Debug.Log("내려찍기 피니쉬 성공");
                player.GetComponent<Player>().TakeDamage(_finishDamage, true);
                player.GetComponent<Player>().Knockback(GetKnockBackPosition(), _knockBackDuration);
                player.GetComponent<Player>().TakeStun(1f);
                _isHit = true;

                // 히트 파티클 생성
                //GameObject hitParticle = ObjectPool.Instance.Spawn("FX_EliteAttack", 1); ;

                //Vector3 hitPos = player.ClosestPoint(_monster.HitPoint.position);
                //hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z - 0.1f);
            }
        }

        if (_isHit)
        {
            //_monster.Ani.SetBool("Takedown_Groggy", true);
        }

        count++;
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
