using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject : MonoBehaviour
{
    private SkillBase skill;
    private SkillManager playerSkillManager;

    public void Initialize(SkillBase skill)
    {
        this.skill = skill;
    }

    public SkillBase GetSkill()
    {
        Destroy(gameObject);

        return skill;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerSkillManager = other.GetComponent<SkillManager>();
            if (playerSkillManager)
            {
                playerSkillManager.InteractObject(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerSkillManager)
            {
                playerSkillManager.DeInteractObject();
            }
        }
    }
}
