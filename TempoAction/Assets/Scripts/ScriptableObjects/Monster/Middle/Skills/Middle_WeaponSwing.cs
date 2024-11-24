using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSwing", menuName = "ScriptableObjects/MiddleMonster/Skill/WeaponSwing", order = 1)]
public class Middle_WeaponSwing : Middle_Skill
{
    private float _coolTime;

    [Header("Hit 포지션")]
    [SerializeField] private Vector3 _hitPoint;
    [Header("Hit 스케일")]
    [SerializeField] private Vector3 _hitScale;

    private Vector3 originSize;
    private Vector3 originPoint;

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
        Debug.Log("휘두르기");

        originSize = _monster.ColliderSize;
        originPoint = _monster.HitPoint.localPosition;

        _monster.HitPoint.localPosition = new Vector3(_hitPoint.x, _hitPoint.y, _hitPoint.z);
        _monster.ColliderSize = new Vector3(_hitScale.x, _hitScale.y, _hitScale.z);

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
    }

    public override void Stay()
    {
        if (!_monster.Ani.GetBool("WeaponSwing"))
        {
            _monster.Ani.SetBool("WeaponSwing", true);
        }
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("WeaponSwing", false);
        _monster.ColliderSize = originSize;
        _monster.HitPoint.localPosition = originPoint;

        IsCompleted = false;
        _coolTime = 0;
    }

    private void Attack()
    {
        TestSound.Instance.PlaySound("Swing");

        GameObject effect = ObjectPool.Instance.Spawn("P_GCHomerunSlash", 1);

        effect.transform.position = _monster.transform.position + new Vector3(0, 0.5f);

        Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        foreach (Collider player in hitPlayer)
        {
            var _player = player.GetComponent<Player>();
            if (_player.IsInvincible) return;

            TestSound.Instance.PlaySound("SwingHit");
            _player.TakeDamage(Info.damage);

            GameObject hitParticle = ObjectPool.Instance.Spawn("FX_HomerunAttack@P", 1);

            Vector3 hitPos = player.ClosestPoint(_monster.HitPoint.position);
            hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z - 0.1f);
        }
    }

    private void Finish()
    {
        _monster.FinishSkill();
    }
}
