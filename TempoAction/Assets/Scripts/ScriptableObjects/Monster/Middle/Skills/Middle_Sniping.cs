using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sniping", menuName = "ScriptableObjects/MiddleMonster/Skill/Sniping", order = 1)]
public class Middle_Sniping : Middle_Skill
{
    [Header("드럼 날라가는 속도")]
    [SerializeField] private float _power = 0;

    private float _coolTime = 0f;
    private GameObject mark;
    private GameObject drum;

    public override void Init(MiddleMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
    }
    public override void Check()
    {
        if (IsCompleted) return;

        if (_coolTime >= _info.coolTime) // 쿨타임 확인
        {
            IsCompleted = true;
        }
        else
        {
            _coolTime += Time.deltaTime;
        }
    }

    public override void Enter()
    {
        Vector3 drumPos = new Vector3(Random.Range(-8, 7), 3, _monster.Player.position.z + 2.5f);
        drum = ObjectPool.Instance.Spawn("Drum", 0);
        drum.transform.position = drumPos;

        mark = ObjectPool.Instance.Spawn("SnipingMark", 0, _monster.Player.transform);
        mark.transform.localPosition = new Vector3(0, 0, -0.2f);
        drum.GetComponent<Drum>().snipingMark = mark;

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
    }

    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Sniping"))
        {
            _monster.Ani.SetBool("Sniping", true);
        }
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("Sniping", false);

        ObjectPool.Instance.Remove(drum);

        drum = null;
        mark = null;
        _coolTime = 0f;
        IsCompleted = false;
    }

    private void Attack()
    {
        if (!mark.gameObject.activeInHierarchy)
        {
            drum.GetComponent<Rigidbody>().AddForce(Vector3.back * _power, ForceMode.Force);
            drum.GetComponent<Drum>().OffMarkSet();
            return;
        }

        drum.GetComponent<Drum>().OffMarkSet();
        if (_monster.Player.GetComponent<Player>().IsInvincible) return;

        _monster.Player.GetComponent<Player>().TakeDamage(_info.damage);
    }

    private void Finish()
    {
        _monster.FinishSkill();
    }
}
