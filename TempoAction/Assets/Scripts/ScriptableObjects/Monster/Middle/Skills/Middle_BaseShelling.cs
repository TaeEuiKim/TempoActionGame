using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseShelling", menuName = "ScriptableObjects/MiddleMonster/Skill/BaseShelling", order = 1)]
public class Middle_BaseShelling : Middle_Skill
{
    [Header("Hit 포지션")]
    [SerializeField] private Vector3 _hitPoint;
    [Header("Hit 스케일")]
    [SerializeField] private Vector3 _hitScale;
    [Header("데미지")]
    [SerializeField] private float _finishDamage;

    [Header("발사 각도")]
    [SerializeField] private float launchAngle = 45;
    [Header("중력 가속도")]
    [SerializeField] private float gravityForce = 9.8f;

    private float _coolTime;

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
            IsCompleted = true;
        }
        else
        {
            _coolTime += Time.deltaTime;
        }
    }

    public override void Enter()
    {
        Debug.Log("곡사포");

        originSize = _monster.ColliderSize;
        originPoint = _monster.HitPoint.localPosition;
        _monster.CharacterModel.localScale = new Vector3(-_monster.Direction, 1, 1);

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
    }

    public override void Stay()
    {
        if (!_monster.Ani.GetBool("BaseShelling"))
        {
            _monster.Ani.SetBool("BaseShelling", true);
        }
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("BaseShelling", false);
        _monster.ColliderSize = originSize;
        _monster.HitPoint.localPosition = originPoint;
    }

    private void Attack()
    {
        GameObject rocket = ObjectPool.Instance.Spawn("LandTraceRocket");

        rocket.transform.position = _monster.transform.position + new Vector3(0, 0.8f);
        rocket.GetComponent<BaseShelling>().SetSetting(_monster.Player, launchAngle, gravityForce);
    }

    private void Finish()
    {
        _monster.FinishSkill();
    }
}
