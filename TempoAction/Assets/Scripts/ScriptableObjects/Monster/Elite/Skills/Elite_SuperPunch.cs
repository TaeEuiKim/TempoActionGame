using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperPunch", menuName = "ScriptableObjects/EliteMonster/Skill/SuperPunch", order = 1)]
public class Elite_SuperPunch : Elite_Skill
{
    private float _coolTime;

    [SerializeField] private float _parringTime;
    [SerializeField] private float _knockBackPower;
    [SerializeField] private float _knockBackDuration;

    private GameObject _punchReadyEffect;

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
    }

    public override void Check()
    {
        if (IsCompleted) return;

        if (_coolTime >= _info.coolTime)
        {
            if (Vector2.Distance(_monster.Player.position, _monster.transform.position) <= _info.range)
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
        Vector2 direction = _monster.Player.position - _monster.transform.position;
        _monster.Direction = direction.x;

        Debug.Log("멀리 치기");

        _punchReadyEffect = ObjectPool.Instance.Spawn("FX_EliteSuperPunchReady");
        _punchReadyEffect.transform.position = _monster.transform.position + new Vector3(0, 0, -0.1f);

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
    }
    public override void Stay()
    {
        if (!_monster.Ani.GetBool("SuperPunch"))
        {
            _monster.Ani.SetBool("SuperPunch", true);
        }
    }
    public override void Exit()
    {
        ObjectPool.Instance.Remove(_punchReadyEffect);
        _monster.Ani.SetBool("SuperPunch", false);

        _coolTime = 0;

        IsCompleted = false;
    }

    private bool CheckParringBox()
    {
        return Physics.CheckBox(_monster.ParringPoint.position, _monster.ParringColliderSize / 2, _monster.ParringPoint.rotation, _monster.PlayerLayer);
    }

    private void Attack()
    {
        
        Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        foreach (Collider player in hitPlayer)
        {
            if (player.GetComponent<Player>().IsInvincible) return;

            Debug.Log("멀리 치기 성공");
            float damage = _monster.Stat.Damage * (_info.damage / 100);
            _monster.Player.GetComponent<Player>().TakeDamage(damage);
            _monster.Player.GetComponent<Player>().Knockback(GetKnockBackPosition(), _knockBackDuration);

            CreateEffect(player);
        }

        
    }

    private void Finish()
    {
        _monster.FinishSkill();
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

    private void CreateEffect(Collider player)
    {
        Vector3 hitPos = player.ClosestPoint(_monster.HitPoint.position);
        // 히트 파티클 생성
        GameObject hitParticle = ObjectPool.Instance.Spawn("FX_EliteAttack", 1);
        hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z - 0.1f);

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
    }
}