using UnityEngine;

public abstract class SkillRunnerBase : ScriptableObject
{
    public SkillData skillData;

    public abstract void Run();
}