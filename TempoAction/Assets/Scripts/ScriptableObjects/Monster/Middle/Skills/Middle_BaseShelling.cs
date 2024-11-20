using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_BaseShelling : Middle_Skill
{
    //[Header("발사 각도")]
    //float abgle;

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
    }

    public override void Exit()
    {
    }

    private void Attack()
    {
        GameObject rocket = ObjectPool.Instance.Spawn("LandTraceRocket");

        rocket.GetComponent<BaseShelling>().SetSetting(_monster.Player);
    }

    private void Finish()
    {

    }
}
