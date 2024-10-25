using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject : MonoBehaviour
{
    public string skillName;
    private ISkillRoot skill;
    private PlayerSkillManager playerSkillManager;
    private Player _player;

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
            _player = other.GetComponent<Player>();

            if (playerSkillManager)
            {
                playerSkillManager.InteractObject(this);
                playerSkillManager.AddSkill(GetSkill());

                _player.SkillObject.gameObject.SetActive(true);
            }
        }
    }
}
