using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillRoot
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>��ų�� �Ҹ���� ����. true = �Ҹ�, false = ����</returns>
    public abstract void UseSkill(CharacterBase skillManager, UnityAction OnEnded = null);
    public abstract int GetSkillId();
    public abstract void SetSkillAdded();
}
