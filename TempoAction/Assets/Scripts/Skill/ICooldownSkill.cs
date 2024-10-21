using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICooldownSkill
{
    public void UpdateTime(float deltaTime);

    public bool IsSkillUsable();
}
