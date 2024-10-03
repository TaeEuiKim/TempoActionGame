using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSkillSlot : SkillSlot
{
    public bool IsUsable(MonsterSkillManager sm)
    {
        var curSkill = skill as MonsterSkill;
        var condition = curSkill.SkillData.SkillTriggerCondition;
        // ������ ������ ���� ó��
        if (condition == Define.SkillTerms.NONE) { return true; }

        List<GameObject> targets = GetTargets(sm);

        // Ÿ���� ���ٰ� �νĵǸ� ���� ó��
        if(targets.Count == 0) { return false; }

        List<GameObject> objsInRange;
        float radius = curSkill.SkillData.SkillTriggerValue * 0.01f; // cm to m

        // ���� ���� �ִ� obj ����Ʈ ���
        objsInRange = targets.Where(
            (obj) =>
            Vector3.Distance(obj.transform.position, sm.transform.position) <= radius
        ).ToList();

        switch (condition)
        {
            case Define.SkillTerms.INRANGE:
                return objsInRange.Count() > 0;    // ���� ���� �ִ� �ֵ��� �ִٸ� true
            case Define.SkillTerms.OUTRANGE:
                return objsInRange.Count() == 0;   // ���� ���� �ִ� �ֵ��� ���ٸ� true
            default:
                return false;
        }
    }

    private List<GameObject> GetTargets(MonsterSkillManager sm)
    {
        var curSkill = skill as MonsterSkill;
        List<GameObject> targets;
        switch (curSkill.SkillData.SkillCastingTarget)
        {
            case Define.SkillTarget.PC:
                targets = CharacterManager.GetCharacter(1 << 11); // player layer
                break;
            case Define.SkillTarget.SELF:
                targets = new List<GameObject>();
                targets.Add(sm.gameObject);
                break;
            case Define.SkillTarget.MON:
                targets = CharacterManager.GetCharacter(1 << 10); // monster layer
                break;
            case Define.SkillTarget.GROUND:
                targets = new List<GameObject>();

                if (Physics.Raycast(new Ray(sm.transform.position, Vector3.down), out RaycastHit hit, 100, 1 << 12))
                {
                    targets.Add(hit.transform.gameObject);
                }
                break;
            case Define.SkillTarget.ALL:
                targets = CharacterManager.GetCharacter(1 << 11 | 1 << 10);
                break;
            case Define.SkillTarget.NONE:
            default:
                targets = new List<GameObject>();
                break;
        }

        return targets;
    }
}
