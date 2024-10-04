using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerSkillSlot : SkillSlot
{
    public KeyCode slotKey;

    public void UseSkillKeyDown(PlayerSkillManager sm)
    {
        if(slotKey == KeyCode.None) { return; }

        if (Input.GetKeyDown(slotKey))
        {
            UseSkillInstant(sm);
        }
    }
}
