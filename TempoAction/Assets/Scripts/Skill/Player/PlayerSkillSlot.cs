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

        if (Skill.UseSkill(character, OnEnded))
        {
            RemoveSkill();
        }
    }

    public void UseSkillKeyDown(CharacterBase cb)
    {
        if(slotKey == KeyCode.None) { return; }

        if (Input.GetKeyDown(slotKey))
        {
            UseSkillInstant(cb);
        }
    }

}
