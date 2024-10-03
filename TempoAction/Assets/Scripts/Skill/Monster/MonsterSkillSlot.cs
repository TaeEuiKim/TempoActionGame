using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MonsterSkillSlot : SkillSlot
{
    [SerializeReference] public MonsterNormalSkillData skillData;

    public void SetSkill()
    {
        MonsterSkill skill = new MonsterSkill(skillData);

        Skill = skill;
    }

    public bool IsUsable(MonsterSkillManager sm)
    {
        // 쿨 대기 중이면 실패 처리
        var curSkill = Skill as MonsterSkill;
        if(curSkill.IsCooldown()) { return false; } 

        // 조건이 없으면 성공 처리
        var condition = curSkill.SkillData.SkillTriggerCondition;
        if (condition == Define.SkillTerms.NONE) { return true; }

        // 타겟이 없다고 인식되면 실패 처리
        List<GameObject> targets = GetTargets(sm);
        if(targets.Count == 0) { return false; }

        List<GameObject> objsInRange;
        float radius = curSkill.SkillData.SkillTriggerValue * 0.01f; // cm to m

        // 범위 내에 있는 obj 리스트 계산
        objsInRange = targets.Where(
            (obj) =>
            Mathf.Abs(obj.transform.position.x - sm.transform.position.x) <= radius
        ).ToList();

        // 조건에 따라 반환
        switch (condition)
        {
            case Define.SkillTerms.INRANGE:
                return objsInRange.Count() > 0;    // 범위 내에 있는 애들이 있다면 true
            case Define.SkillTerms.OUTRANGE:
                return objsInRange.Count() == 0;   // 범위 내에 있는 애들이 없다면 true
            default:
                return false;
        }
    }

    private List<GameObject> GetTargets(MonsterSkillManager sm)
    {
        var curSkill = Skill as MonsterSkill;
        List<GameObject> targets;
        switch (curSkill.SkillData.SkillCastingTarget)
        {
            case Define.SkillTarget.PC:
                targets = CharacterManager.Instance.GetCharacter(1 << 11); // player layer
                break;
            case Define.SkillTarget.SELF:
                targets = new List<GameObject>
                {
                    sm.gameObject
                };
                break;
            case Define.SkillTarget.MON:
                targets = CharacterManager.Instance.GetCharacter(1 << 10); // monster layer
                break;
            case Define.SkillTarget.GROUND:
                targets = new List<GameObject>();

                if (Physics.Raycast(new Ray(sm.transform.position, Vector3.down), out RaycastHit hit, 100, 1 << 12))
                {
                    targets.Add(hit.transform.gameObject);
                }
                break;
            case Define.SkillTarget.ALL:
                targets = CharacterManager.Instance.GetCharacter(1 << 11 | 1 << 10);
                break;
            case Define.SkillTarget.NONE:
            default:
                targets = new List<GameObject>();
                break;
        }

        return targets;
    }
}
