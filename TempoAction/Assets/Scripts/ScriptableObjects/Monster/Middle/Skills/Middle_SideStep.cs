using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SideStep", menuName = "ScriptableObjects/MiddleMonster/Skill/SideStep", order = 1)]
public class Middle_SideStep : Middle_Skill
{
    [Header("왼쪽 포인트")]
    [SerializeField] private Vector3 leftPoint;
    [Header("오른쪽 포인트")]
    [SerializeField] private Vector3 rightPoint;

    private float _coolTime = 0;
    private int count = 0;

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
            if (_monster.IsHit)
            {
                IsCompleted = true;
                _monster.IsHit = false;
            }
        }
        else
        {
            _coolTime += Time.deltaTime;
        }
    }

    public override void Enter()
    {
        Debug.Log("후퇴");

        count = 0;
        if (_monster.IsLeftDirection())
        {
            _monster.transform.DOMoveX(_monster.transform.position.x + 4, 1f).OnComplete(() =>
            {
                _monster.transform.position = leftPoint + new Vector3(-4f, 0, 0);

                _monster.Ani.SetBool("SideStepCount", true);
            });
        }
        else
        {
            _monster.transform.DOMoveX(_monster.transform.position.x - 4, 1f).OnComplete(() =>
            {
                _monster.transform.position = rightPoint + new Vector3(4f, 0, 0);

                _monster.Ani.SetBool("SideStepCount", true);
            });
        }

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
    }

    public override void Stay()
    {
        if (!_monster.Ani.GetBool("SideStep"))
        {
            _monster.Ani.SetBool("SideStep", true);
        }
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("SideStep", false);
        _monster.Ani.SetBool("SideStepCount", false);
        _monster.IsHit = false;

        IsCompleted = false;
    }

    private void Attack()
    {
        _monster.transform.DOKill();

        TestSound.Instance.PlaySound("Step");

        if (_monster.IsLeftDirection())
        {
            _monster.Direction = -_monster.Direction;
            _monster.transform.DOMoveX(leftPoint.x, 1f);
        }
        else
        {
            _monster.Direction = -_monster.Direction;
            _monster.transform.DOMoveX(rightPoint.x + 0.5f, 1f);
        }
    }

    private void Finish()
    {
        _monster.transform.position = new Vector3(_monster.transform.position.x, _monster.transform.position.y, -8.8f);

        _monster.FinishSkill();
    }
}
