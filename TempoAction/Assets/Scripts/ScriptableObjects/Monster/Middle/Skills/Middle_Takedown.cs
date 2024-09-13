using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Takedown", menuName = "ScriptableObjects/MiddleMonster/Skill/Takedown", order = 1)]
public class Middle_Takedown : Middle_Skill
{
    [SerializeField] int _attackCount;
    [SerializeField] float _finishDamage;

    public override void Init(MiddleMonster monster)
    {
        base.Init(monster);
    }

    public override void Check()
    {
        if (IsCompleted) return;
    }

    public override void Enter()
    {
        Debug.Log("³»·ÁÂï±â");


    }

    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Takedown"))
        {
            _monster.Ani.SetBool("Takedown", true);
        }
    }

    public override void Exit()
    {
        
    }

    private void Hit()
    {

    }

    private void Finish()
    {
        _monster.FinishSkill();
    }
}
