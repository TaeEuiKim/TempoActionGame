using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillRoot
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>��ų�� �Ҹ���� ����. true = �Ҹ�, false = ����</returns>
    public abstract bool UseSkill(CharacterBase skillManager);
}
