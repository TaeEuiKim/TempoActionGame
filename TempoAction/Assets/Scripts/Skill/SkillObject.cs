using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject : MonoBehaviour
{
    private ISkillRoot skill;
    private PlayerSkillManager playerSkillManager;

    public void Initialize(ISkillRoot skill)
    {
        this.skill = skill;
    }

    public ISkillRoot GetSkill()
    {
        Destroy(gameObject);

        return skill;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerSkillManager = other.GetComponent<PlayerSkillManager>();
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
