using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NormalSkill : SkillBase
{
    [field: SerializeField] public float cooldown { get; private set; }

    private float curTime;

    public NormalSkill(int id, string name) : base(id, name)
    {
        OnSkillAttack.AddListener(() => { Debug.Log("Invoke Normal Skill"); });
    }

    public virtual void UpdateTime(float deltaTime)
    {
        curTime += deltaTime;
    }

    public bool IsCooldown() => cooldown > curTime;

    public override bool UseSkill()
    {
        bool isRemove = false;
        if (IsCooldown()) { isRemove = true; }

        OnSkillAttack.Invoke();

        curTime = 0;

        return isRemove;
    }
}
