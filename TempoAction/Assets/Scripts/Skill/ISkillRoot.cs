using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ISkillRoot
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>��ų�� �Ҹ���� ����. true = �Ҹ�, false = ����</returns>
    public abstract bool UseSkill(CharacterBase skillManager, UnityAction OnEnded = null);
    public abstract int GetSkillId();
}
