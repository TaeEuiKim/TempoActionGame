using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Homerun", menuName = "ScriptableObjects/MiddleMonster/Skill/Homerun", order = 1)]
public class Middle_Homerun : Middle_Skill
{
    private float _coolTime;

    [SerializeField] private float _knockBackPower;
    [SerializeField] private float _knockBackDuration;

    public override void Init(MiddleMonster monster)
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
        Debug.Log("홈런");
        _monster.ColliderSize = new Vector3(_monster.ColliderSize.x * 2f, _monster.ColliderSize.y * 1.5f, _monster.ColliderSize.z);
        CoroutineRunner.Instance.StartCoroutine(MoveToPlayer());

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
    }

    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Homerun"))
        {
            _monster.Ani.SetBool("Homerun", true);
        }
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("Homerun", false);
        _monster.ColliderSize = new Vector3(1, 1.5f, 1);
        _coolTime = 0;

        IsCompleted = false;
    }

    IEnumerator MoveToPlayer()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        float dis = Vector3.Distance(_monster.transform.position, _monster.Player.transform.position - new Vector3(_monster.Direction, 0, 0));

        if (_monster.MonsterModel.transform.localScale.x < 0)
        {
            _monster.transform.DOMoveX(_monster.transform.position.x + dis, 0.6f);
        }
        else if (_monster.MonsterModel.transform.localScale.x > 0)
        {
            _monster.transform.DOMoveX(_monster.transform.position.x - dis, 0.6f);
        }
    }

    private void Attack()
    {
        Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        foreach (Collider player in hitPlayer)
        {
            if (player.GetComponent<Player>().IsInvincible) return;

            Debug.Log("홈런 성공");
            player.GetComponent<Player>().TakeDamage(_info.damage, true);
            player.GetComponent<Player>().Knockback(GetKnockBackPosition(), _knockBackDuration);
            player.GetComponent<Player>().TakeStun(1f);

            // 히트 파티클 생성
            GameObject hitParticle = ObjectPool.Instance.Spawn("FX_HomerunAttack@P", 1); ;

            Vector3 hitPos = player.ClosestPoint(_monster.HitPoint.position);
            hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z - 0.1f);
        }
    }

    private Vector3 GetKnockBackPosition()
    {
        RaycastHit hit;
        Vector3 pos = _monster.transform.position;
        pos.y = 1.54f;

        if (Physics.Raycast(pos, Vector2.right * _monster.Direction, out hit, _knockBackPower * _knockBackDuration, _monster.WallLayer))
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
