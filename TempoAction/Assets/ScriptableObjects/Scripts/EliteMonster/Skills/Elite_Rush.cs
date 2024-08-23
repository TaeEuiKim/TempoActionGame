using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "Rush", menuName = "ScriptableObjects/EliteMonster/Skill/Rush", order = 1)]
public class Elite_Rush : Elite_Skill
{
    private float _coolTime;

    private Tweener _rushTween;

    [SerializeField] private float _rushDistance;
    [SerializeField] private float _rushDuration;
    [SerializeField] AnimationCurve customCurve; // 사용자 정의 애니메이션 커브

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
    }

    public override bool Check()
    {
        if (_coolTime >= _info.coolTime) // 쿨타임 확인
        {
            return true;
        }
        else
        {
            _coolTime += Time.deltaTime;
        }

        return false;
    }

    public override void Enter()
    {
        Debug.Log("돌진");

        _monster.Direction = _monster.Player.position.x - _monster.transform.position.x; // 플레이어 바라보기

        // 돌진
        _rushTween = _monster.transform.DOMoveX(_monster.transform.position.x + _rushDistance * _monster.Direction, _rushDuration).SetEase(customCurve).OnUpdate(() =>
        {
            if (CheckHit()) // 플레이어와 충돌 시
            {
                _monster.FinishSkill();
                _rushTween.Kill();
            }

        }).OnComplete(() => { _monster.FinishSkill();  }); // 이동이 끝난 후

    }
    public override void Stay()
    {
    }

    public override void Exit()
    {
        _coolTime = 0;
    }

    // 충돌 확인 함수
    private bool CheckHit()
    {
        if (Physics.CheckBox(_monster.transform.position, _monster.RushColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer))
        {
            float damage = _monster.Stat.Damage * (_info.damage / 100);
            _monster.Player.GetComponent<Player>().TakeDamage(damage);

            return true;
        }
        else if (Physics.CheckBox(_monster.transform.position, _monster.RushColliderSize / 2, _monster.HitPoint.rotation, _monster.WallLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
