using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseShelling", menuName = "ScriptableObjects/MiddleMonster/Skill/BaseShelling", order = 1)]
public class Middle_BaseShelling : Middle_Skill
{
    [Header("몬스터에게 들어가는 데미지")]
    [SerializeField] private float _finishDamage;

    [Header("발사 각도")]
    [SerializeField] private float launchAngle;
    [Header("중력 가속도")]
    [SerializeField] private float gravityForce = 9.8f;
    [Header("발사하는 힘의 배율")]
    [SerializeField] private float launchPower;

    private float _coolTime;

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
    }

    private void Attack()
    {
        GameObject rocket = ObjectPool.Instance.Spawn("LandTraceRocket");

        rocket.transform.position = _monster.transform.position + new Vector3(0.8f * -_monster.Direction, 1.2f);
        rocket.GetComponent<BaseShelling>().SetSetting(_monster.Player, launchPower, launchAngle, gravityForce, Info.damage, _finishDamage);
    }

    private void Finish()
    {
        IsCompleted = false;
        _coolTime = 0;

        _monster.FinishSkill();
    }
}
