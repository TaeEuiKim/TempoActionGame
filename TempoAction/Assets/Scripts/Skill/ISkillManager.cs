using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillManager
{
    public int MaxSkillSlot { get; }
    public SkillSlot[] SkillSlots { get; }

    public void Initialize();
    public void OnUpdate(CharacterBase characterBase);
}
