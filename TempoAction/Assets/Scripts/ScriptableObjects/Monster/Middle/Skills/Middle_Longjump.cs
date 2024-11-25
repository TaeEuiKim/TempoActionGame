using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Longjump", menuName = "ScriptableObjects/MiddleMonster/Skill/Longjump", order = 1)]
public class Middle_Longjump : Middle_Skill
{
    private float _coolTime = 0f;
    private bool isFlying = false;
    private float attackPos;

    [SerializeField] private float _knockBackPower;
    [SerializeField] private float _knockBackDuration;
    [Header("Hit 포지션")]
    [SerializeField] private Vector3 _hitPoint;
    [Header("Hit 스케일")]
    [SerializeField] private Vector3 _hitScale;
    [Header("피니쉬 공격 데미지 체력(%)")]
    [SerializeField] float _finishDamage;

    private Vector3 originSize;
    private Vector3 originPoint;

    public override void Init(MiddleMonster monster)
    {
        base.Init(monster);

        _coolTime = 0f;
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
        attackPos = _monster.Player.transform.position.x - _monster.Direction;
        originSize = _monster.ColliderSize;
        originPoint = _monster.HitPoint.localPosition;
        _monster.CharacterModel.localScale = new Vector3(-_monster.Direction, 1, 1);

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
    }

    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Longjump"))
        {
            _monster.Ani.SetBool("Longjump", true);
        }

        //if (!isHit && isFlying)
        //{
        //    Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        //    foreach (Collider player in hitPlayer)
        //    {
        //        if (player.GetComponent<Player>().IsInvincible) return;

        //        Debug.Log("멀리뛰기 성공");
        //        player.GetComponent<Player>().TakeDamage(_info.damage, true);
        //        isHit = true;
        //    }
        //}
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("Longjump", false);
        _monster.ColliderSize = originSize;
        _monster.HitPoint.localPosition = originPoint;
        _coolTime = 0;

        IsCompleted = false;
    }

    private void Attack()
    {
        if (!isFlying)
        {
            TestSound.Instance.PlaySound("Longjump_Jump");
            TestSound.Instance.PlaySound("Longjump_Voice");

            GameObject hitParticle = ObjectPool.Instance.Spawn("FX_ChungJump@P", 1);

            hitParticle.transform.position = new Vector3(_monster.transform.position.x, 1.4f, _monster.transform.position.z);

            _monster.transform.DOMoveY(15f, 1.2f);
            _monster.Rb.useGravity = false;

            isFlying = true;
        }
        else
        {
            attackPos = _monster.Player.transform.position.x;
            _monster.CharacterModel.localScale = new Vector3(_monster.transform.position.x - _monster.Player.transform.position.x > 0 ? 1f : -1f, 1, 1);

            GameObject hitParticle2 = ObjectPool.Instance.Spawn("FX_ChungLandingPoint", 1);

            hitParticle2.transform.position = new Vector3(attackPos + 1.4f * -_monster.Direction, 1.3f, _monster.transform.position.z);
        }
    }

    IEnumerator FinishTimer()
    {
        yield return new WaitForSeconds(3f);

        _monster.FinishSkill();
    }

    private void Finish()
    {
        TestSound.Instance.PlaySound("Longjump_Fall");

        _monster.Rb.useGravity = true;

        Vector3 pos = _monster.transform.position;
        if (_monster.CharacterModel.localScale.x > 0)
        {
            pos.x = attackPos + 2.4f;
        }
        else
        {
            pos.x = attackPos - 2.4f;
        }
        _monster.transform.position = pos;

        _monster.transform.DOKill();
        _monster.transform.DOMoveY(1, 0.1f).OnComplete(() =>
        {
            _monster.Rb.velocity = Vector3.zero;
        });

        Vector3 hitPos = new Vector3(attackPos, 0.97f, _monster.HitPoint.position.z);

        _monster.HitPoint.localPosition = new Vector3(_hitPoint.x, _hitPoint.y);
        _monster.ColliderSize = new Vector3(_hitScale.x, _hitScale.y, _hitScale.z);
        isFlying = false;

        GameObject hitParticle = ObjectPool.Instance.Spawn("FX_ChungLanding@P", 1);

        CameraController.Instance.SceneShaking();
        TestSound.Instance.PlaySound("Longjump_Kang");

        hitParticle.transform.position = new Vector3(attackPos + 1.4f * -_monster.Direction, 1.2f, _monster.transform.position.z);

        Collider[] hitPlayer = Physics.OverlapBox(hitPos, _hitScale / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        foreach (Collider player in hitPlayer)
        {
            Player p = player.GetComponent<Player>();

            if (p.IsInvincible) return;

            Debug.Log("멀리뛰기 피니쉬 성공");
            p.TakeDamage(_finishDamage);

            int dir = 1;
            if ((_monster.transform.position - _monster.Player.transform.position).x > 0)
            {
                dir = -1;
            }

            p.TakeStun(1f, dir);
        }

        CoroutineRunner.Instance.StartCoroutine(FinishTimer());
    }
}
