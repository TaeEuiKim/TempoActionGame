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
    private float attackPos;

    [SerializeField] private float _knockBackPower;
    [SerializeField] private float _knockBackDuration;
    [Header("Hit ������")]
    [SerializeField] private Vector3 _hitPoint;
    [Header("Hit ������")]
    [SerializeField] private Vector3 _hitScale;
    [Header("�ǴϽ� ���� ������ ü��(%)")]
    [SerializeField] float _finishDamage;

    private Vector3 originSize;
    private Vector3 originPoint;

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

        if (_coolTime >= _info.coolTime) // ��Ÿ�� Ȯ��
        {
            if (Vector2.Distance(_monster.Player.position, _monster.transform.position) <= _info.range) // �Ÿ� Ȯ��
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
        Debug.Log("�ָ��ٱ�");
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

        //        Debug.Log("�ָ��ٱ� ����");
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

        isHit = false;
        IsCompleted = false;
    }

    private void Attack()
    {
        if (!isFlying)
        {
            _monster.transform.DOMoveY(15f, 1.2f);
            _monster.Rb.useGravity = false;

            GameObject hitParticle = ObjectPool.Instance.Spawn("FX_ChungJump@P", 1);

            hitParticle.transform.position = new Vector3(_monster.transform.position.x, 1.7f, _monster.transform.position.z);

            isFlying = true;
        }
        else
        {
            attackPos = _monster.Player.transform.position.x - _monster.Direction;

            GameObject hitParticle2 = ObjectPool.Instance.Spawn("FX_ChungLandingPoint", 1);

            hitParticle2.transform.position = new Vector3(attackPos + _monster.Direction * 1.5f, 1.3f, _monster.transform.position.z);
        }
    }

    IEnumerator FinishTimer()
    {
        yield return new WaitForSeconds(3f);

        _monster.FinishSkill();
    }

    private void Finish()
    {
        _monster.Rb.useGravity = true;

        Vector3 pos = _monster.transform.position;
        pos.x = attackPos;
        _monster.transform.position = pos;

        _monster.transform.DOMoveY(0.97f, 0.3f);

        Vector3 hitPos = new Vector3(_monster.HitPoint.position.x, 0.97f, _monster.HitPoint.position.z);

        _monster.HitPoint.localPosition = new Vector3(_hitPoint.x, _hitPoint.y);
        _monster.ColliderSize = new Vector3(_hitScale.x, _hitScale.y, _hitScale.z);
        isFlying = false;

        GameObject hitParticle = ObjectPool.Instance.Spawn("FX_ChungLanding@P", 1);

        hitParticle.transform.position = new Vector3(_monster.transform.position.x + (_monster.Direction * 1.5f), 1.2f, _monster.transform.position.z);

        Collider[] hitPlayer = Physics.OverlapBox(hitPos, _hitScale / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        foreach (Collider player in hitPlayer)
        {
            Player p = player.GetComponent<Player>();

            if (p.IsInvincible) return;

            Debug.Log("�ָ��ٱ� �ǴϽ� ����");
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
