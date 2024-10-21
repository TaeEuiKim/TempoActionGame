using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerSkillSlot : SkillSlot
{
    public KeyCode slotKey;

    public override void UseSkillInstant(CharacterBase character, UnityAction OnEnded = null)
    {
        if (Skill == null) { return; }
        Skill.UseSkill(character, OnEnded);
    }

    public void UseSkillKeyDown(CharacterBase cb, bool isSkill)
    {
        if(slotKey == KeyCode.None) { return; }
        if (!isSkill) { return; }

        if (Input.GetKeyDown(slotKey))
        {
            if (cb.TryGetComponent<PlayerSkillManager>(out var s))
            {
                s.SetIsSkill(false);
            }
            UseSkillInstant(cb);
        }
    }

}
