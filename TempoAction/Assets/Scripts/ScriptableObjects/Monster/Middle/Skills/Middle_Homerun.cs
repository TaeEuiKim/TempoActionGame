using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Homerun", menuName = "ScriptableObjects/MiddleMonster/Skill/Homerun", order = 1)]
public class Middle_Homerun : Middle_Skill
{
    private float _coolTime;
    private bool isFront = false;

    [Header("전방 Hit 포지션")]
    [SerializeField] private Vector3 _frontHitPoint;
    [Header("전방 Hit 스케일")]
    [SerializeField] private Vector3 _frontHitScale;
    [SerializeField] private float _frontKnockBackPower;
    [SerializeField] private float _frontKnockBackDuration;

    [Header("후방 Hit 포지션")]
    [SerializeField] private Vector3 _backHitPoint;
    [Header("후방 Hit 스케일")]
    [SerializeField] private Vector3 _backHitScale;
    [SerializeField] private float _backKnockBackPower;
    [SerializeField] private float _backKnockBackDuration;

    private Vector3 originSize;
    private Vector3 orginPoint;

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
        originSize = _monster.ColliderSize;
        orginPoint = _monster.HitPoint.localPosition;
        _monster.HitPoint.localPosition = new Vector3(_backHitPoint.x, _backHitPoint.y);
        _monster.ColliderSize = new Vector3(_backHitScale.x, _backHitScale.y, _backHitScale.z);
        _monster.CharacterModel.localScale = new Vector3(-_monster.Direction, 1, 1);
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
        _monster.ColliderSize = originSize;
        _monster.HitPoint.localPosition = orginPoint;
        _coolTime = 0;

        isFront = false;
        IsCompleted = false;
    }

    IEnumerator MoveToPlayer()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        float dis = Vector3.Distance(_monster.transform.position, _monster.Player.transform.position - new Vector3(_monster.Direction, 0, 0));

        if (_monster.CharacterModel.transform.localScale.x < 0)
        {
            _monster.transform.DOMoveX(_monster.transform.position.x + dis, 0.6f);
        }
        else if (_monster.CharacterModel.transform.localScale.x > 0)
        {
            _monster.transform.DOMoveX(_monster.transform.position.x - dis, 0.6f);
        }

        TestSound.Instance.PlaySound("Homerun_Dash");

    }

    private void Attack()
    {
        TestSound.Instance.PlaySound("Homerun_Swing");
        TestSound.Instance.PlaySound("Homerun_Voice");

        if (!isFront)
        {
            _monster.HitPoint.localPosition = new Vector3(_backHitPoint.x, _backHitPoint.y);
            _monster.ColliderSize = new Vector3(_backHitScale.x, _backHitScale.y, _backHitScale.z);

            Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _backHitScale / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

            foreach (Collider player in hitPlayer)
            {
                if (player.GetComponent<Player>().IsInvincible) return;

                Debug.Log("후방 홈런 성공");
                player.GetComponent<Player>().TakeDamage(_info.damage);
                player.GetComponent<Player>().Knockback(GetKnockBackPosition(), _backKnockBackDuration);

                int dir = 1;
                if ((_monster.transform.position - _monster.Player.transform.position).x > 0)
                {
                    dir = -1;
                }
                player.GetComponent<Player>().TakeStun(1f, dir);

                // 히트 파티클 생성
                GameObject hitParticle = ObjectPool.Instance.Spawn("FX_HomerunAttack@P", 1);

                Vector3 hitPos = player.ClosestPoint(_monster.HitPoint.position);
                hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z - 0.1f);

                TestSound.Instance.PlaySound("Homerun_Hit");
            }

            isFront = true;
        }
        else
        {
            _monster.HitPoint.localPosition = new Vector3(_frontHitPoint.x, _frontHitPoint.y);
            _monster.ColliderSize = new Vector3(_frontHitScale.x, _frontHitScale.y, _frontHitScale.z);

            Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _frontHitScale / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

            foreach (Collider player in hitPlayer)
            {
                if (player.GetComponent<Player>().IsInvincible) return;

                Debug.Log("전방 홈런 성공");
                player.GetComponent<Player>().TakeDamage(_info.damage);
                player.GetComponent<Player>().Knockback(GetKnockBackPosition(), _frontKnockBackDuration);

                int dir = 1;
                if ((_monster.transform.position - _monster.Player.transform.position).x > 0)
                {
                    dir = -1;
                }

                player.GetComponent<Player>().TakeStun(1f, dir);

                // 히트 파티클 생성
                GameObject hitParticle = ObjectPool.Instance.Spawn("FX_HomerunAttack@P", 1); ;

                Vector3 hitPos = player.ClosestPoint(_monster.HitPoint.position);
                hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z - 0.1f);

                TestSound.Instance.PlaySound("Homerun_Hit");
            }
        }
    }

    private Vector3 GetKnockBackPosition()
    {
        RaycastHit hit;
        Vector3 pos = _monster.transform.position;
        pos.y = 2.159f;

        if (!isFront)
        {
            if (Physics.Raycast(pos, Vector2.right * -_monster.Direction, out hit, _backKnockBackPower * _backKnockBackDuration, _monster.WallLayer))
            {
                return hit.point;
            }

            Vector3 target = new Vector3(pos.x + ((Vector2.right * -_monster.Direction) * (_backKnockBackPower * _backKnockBackDuration)).x, pos.y, pos.z);
            return target;
        }
        else
        {
            if (Physics.Raycast(pos, Vector2.right * _monster.Direction, out hit, _frontKnockBackPower * _frontKnockBackDuration, _monster.WallLayer))
            {
                return hit.point;
            }

            Vector3 target = new Vector3(pos.x + ((Vector2.right * _monster.Direction) * (_frontKnockBackPower * _frontKnockBackDuration)).x, pos.y, pos.z);
            return target;
        }

    }

    private void Finish()
    {
        _monster.FinishSkill();
    }
}
