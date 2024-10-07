using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopySkill : MonoBehaviour
{
    private Image[] playerMainIcons;
    private Image[] playerSubIcons;
    private SkillSlot[] copySkillSlots;
    private Queue<ISkillRoot> copyReserveSlots;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SaveSkillSlots(SkillSlot[] skillSlots, Queue<ISkillRoot> reserveSlots, Image[] main, Image[] sub)
    {
        copySkillSlots = skillSlots;
        copyReserveSlots = reserveSlots;
        playerMainIcons = main;
        playerSubIcons = sub;
        Debug.Log(playerMainIcons[0].sprite);
    }

    public SkillSlot[] LoadSkillSlots()
    {
        if (copySkillSlots == null)
        {
            return null;
        }
        return copySkillSlots;
    }

    public Queue<ISkillRoot> LoadReserveSlots()
    {
        return copyReserveSlots;
    }

    public Image[] LoadMainIcon()
    {
        return playerMainIcons;
    }

    public Image[] LoadSubIcon()
    {
        return playerSubIcons;
    }
}
