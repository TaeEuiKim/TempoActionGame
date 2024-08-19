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

    public override void Check()
    {
        if (_isCompleted) return;

        if (_coolTime >= _info.coolTime) // 쿨타임 확인
        {
            _coolTime = 0;
            _isCompleted = true;
        }
        else
        {
            _coolTime += Time.deltaTime;
        }
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
                float damage = _monster.Stat.AttackDamage * (_info.damage / 100);
                _monster.Player.GetComponent<Player>().Stat.TakeDamage(damage);

                _monster.CurrentSkill = null;
                _rushTween.Kill();
            }

        }).OnComplete(() => { _monster.CurrentSkill = null; }); // 이동이 끝난 후

    }
    public override void Stay()
    {

    }

    public override void Exit()
    {
        _monster.ResetSkill();
        _isCompleted = false;
    }

    // 충돌 확인 함수
    private bool CheckHit()
    {
        return Physics.CheckBox(_monster.transform.position, _monster.RushColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);
    }
}
