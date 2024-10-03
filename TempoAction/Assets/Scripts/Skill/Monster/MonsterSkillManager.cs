using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSkillManager : MonoBehaviour, ISkillManager
{
    [field: SerializeField] public int MaxSkillSlot { get; protected set; }

    [field: SerializeField] public SkillSlot[] SkillSlots {  get; protected set; }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (SkillSlots != null && SkillSlots?.Length != MaxSkillSlot)
        {
            var oldSlots = SkillSlots;
            SkillSlots = new MonsterSkillSlot[MaxSkillSlot];

            for (int i = 0; i < oldSlots.Length; i++)
            {
                if (i >= SkillSlots.Length) { continue; }

                SkillSlots[i] = oldSlots[i];
            }
        }
    }

    public SkillSlot[] GetUsableSkillSlots()
    {
        return SkillSlots.Where((slot) => ((MonsterSkillSlot)slot).IsUsable(this)).ToArray();
    }
}