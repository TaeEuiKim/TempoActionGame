using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shelling", menuName = "ScriptableObjects/MiddleMonster/Skill/Shelling", order = 1)]
public class Middle_Shelling : Middle_Skill
{
    private float _coolTime = 0f;

    public override void Init(MiddleMonster monster)
    {
        base.Init(monster);
    }

    public override void Check()
    {
        if (IsCompleted) return;

        if (_coolTime >= _info.coolTime) // ÄğÅ¸ÀÓ È®ÀÎ
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
        Debug.Log("ÆøÅº ÅõÇÏ");

    }
    public override void Stay()
    {

    }

    public override void Exit()
    {

    }

    private void SpawnRocket()
    {

    }
}
