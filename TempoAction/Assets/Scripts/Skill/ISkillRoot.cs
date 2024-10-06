using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillRoot
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>스킬이 소멸될지 여부. true = 소멸, false = 유지</returns>
    public abstract bool UseSkill(CharacterBase skillManager);
}
