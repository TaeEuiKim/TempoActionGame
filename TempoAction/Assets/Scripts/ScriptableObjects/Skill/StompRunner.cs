using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StompRunner", menuName = "ScriptableObjects/Skill/Runner/StompRunner", order = 1)]
public class StompRunner : SkillRunnerBase
{
    public override IEnumerator SkillCoroutine(CharacterBase character)
    {
        Rigidbody rigid = character.Rb;
        rigid.useGravity = false;

        float movingDistance = skillData.SkillEffectValue * SkillData.cm2m;

        var targets = GetTargets(character, skillData.SkillCastingTarget);

        Vector3 initialPos = character.transform.position;
        Vector3 targetPos = targets[0].transform.position ;
        Vector3 direction = (targetPos - initialPos).normalized;

        // ��Ʈ�ڽ�
        character.ColliderManager.SetActiveCollider(false, Define.ColliderType.PERSISTANCE);

        // ����
        yield return preDelayWFS;

        // ���� �� ����
        float curTime = 0;
        float regenTime = skillData.SkillRegenTime * SkillData.Time2Second;

        List<CharacterBase> hittedCharacters = new List<CharacterBase>();

        targetPos = GetTargetPosByCoillision(initialPos, direction, targetPos, movingDistance);

        // ������ � ����
        while (curTime <= regenTime)
        {
            yield return null;

            curTime += Time.deltaTime;
            float completeRatio = curTime / regenTime;

            /// �ְ� ���̱��� ������ ���Ŀ� �浹 ó���� ����
            if(completeRatio > 0.5f)
            {
                Vector3 rayOrigin = character.transform.position; // ������ Bottom�� �������� �ؾ� �Ʒ��� �� �� ����.
                Ray ray = new Ray(rayOrigin, Vector3.down);
                Debug.DrawRay(rayOrigin, Vector3.down, Color.blue);
                float collisiionDepth = skillData.SkillHitboxSize * SkillData.cm2m;
                int layerMask = SkillTargetToLayerMask(skillData.SkillCastingTarget);
                if (Physics.Raycast(ray, out RaycastHit characterHit, collisiionDepth, layerMask))
                {
                    hittedCharacters.Add(characterHit.transform.GetComponent<CharacterBase>());
                }
            }

            Vector3 nextPos = EvaluateParabola(initialPos, targetPos, 3, completeRatio); // 3�� �ӽ� �� ���� ���� ������ ���� �ʿ�
            character.transform.position = nextPos;
        }

        // ĳ���� Ÿ��
        foreach (var hittedCharacter in hittedCharacters.Distinct())
        {
            float damageAmount = skillData.SkillDamage * character.Stat.Damage;

            hittedCharacter.TakeDamage(damageAmount);
        }

        // �ʱ�ȭ
        character.transform.position = targetPos;
        rigid.velocity = Vector3.zero;
        character.ColliderManager.SetActiveCollider(true, Define.ColliderType.PERSISTANCE);
        rigid.useGravity = true;
        Debug.Log("Stomp End");
    }

    // ������ ��� �ϰ� ���ִ� �޼ҵ�
    // normalizedTime ���� ���� start���� end������ ������ �� ��� ��ġ�� �ִ����� ��ȯ�Ѵ�.
    private Vector3 EvaluateParabola(Vector3 start, Vector3 end, float height, float normalizedTime)
    {
        float quadratic = -4 * height * normalizedTime * normalizedTime + 4 * height * normalizedTime;

        Vector3 vec = Vector3.Lerp(start, end, normalizedTime);

        return new Vector3(vec.x, quadratic + start.y, start.z);
    }
}
