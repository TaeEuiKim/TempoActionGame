using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*public class InstantSkill : SkillBase<SkillData>
{
    public InstantSkill(SkillData skillData) : base(skillData)
    {
        OnSkillAttack.AddListener((cb) => { Debug.Log("Invoke Instant Skill"); });
    }

    public override bool UseSkill(CharacterBase cb)
    {
        OnSkillAttack.Invoke(cb);

        return true;
    }
}
*/

public class InstantSkill : SkillBase
{
    public InstantSkill(SkillRunnerBase skillRunner) : base(skillRunner)
    {
        OnSkillAttack.AddListener((cb) => { Debug.Log("Invoke Instant Skill"); });
    }

    public override bool UseSkill(CharacterBase character, UnityAction OnEnded = null)
    {
        SkillRunner.Run(character, OnEnded);

        return true;
    }

    public override int GetSkillId()
    {
        return -1;
    }
}
