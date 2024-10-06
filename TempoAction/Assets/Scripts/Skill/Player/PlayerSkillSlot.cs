using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerSkillSlot : SkillSlot
{
    public KeyCode slotKey;

    public override void UseSkillInstant(CharacterBase character)
    {
        if (Skill == null) { return; }

        if (Skill.UseSkill(character))
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
