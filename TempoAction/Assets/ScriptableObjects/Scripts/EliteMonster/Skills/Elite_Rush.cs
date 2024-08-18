using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "Rush", menuName = "ScriptableObjects/EliteMonster/Skill/Rush", order = 1)]
public class Elite_Rush : Elite_Skill
{
    private Tweener _rushTween;

    [SerializeField] private float _rushDistance;
    [SerializeField] private float _rushDuration;
    [SerializeField] AnimationCurve customCurve; // 사용자 정의 애니메이션 커브

    private float _coolTime;

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
    }

    public override void Check()
    {
        if (_isCompleted) return;

        if (_coolTime >= _info.coolTime)
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

        float dir = _monster.Player.position.x - _monster.transform.position.x;
        _monster.Direction = dir;

        _rushTween = _monster.transform.DOMoveX(_monster.transform.position.x + _rushDistance * _monster.Direction, _rushDuration).SetEase(customCurve).OnUpdate(() =>
        {
            if (CheckHit())
            {
                float damage = _monster.Stat.AttackDamage * (_info.damage / 100);
                _monster.Player.GetComponent<Player>().Stat.TakeDamage(damage);

                _monster.CurrentSkill = null;
                _rushTween.Kill();
            }

        }).OnComplete(() => { _monster.CurrentSkill = null; });

    }
    public override void Stay()
    {

    }

    public override void Exit()
    {

        _monster.ResetSkill();
        _isCompleted = false;
    }

    private bool CheckHit()
    {
        return Physics.CheckBox(_monster.transform.position, _monster.RushColSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);
    }
}
