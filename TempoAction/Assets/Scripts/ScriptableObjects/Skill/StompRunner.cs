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

        // 히트박스
        character.ColliderManager.SetActiveCollider(false, Define.ColliderType.PERSISTANCE);

        // 선딜
        yield return preDelayWFS;

        // 점프 및 낙하
        float curTime = 0;
        float regenTime = skillData.SkillRegenTime * SkillData.Time2Second;

        List<CharacterBase> hittedCharacters = new List<CharacterBase>();

        targetPos = GetTargetPosByCoillision(initialPos, direction, targetPos, movingDistance);

        // 포물선 운동 시작
        while (curTime <= regenTime)
        {
            yield return null;

            curTime += Time.deltaTime;
            float completeRatio = curTime / regenTime;

            /// 최고 높이까지 도달한 이후에 충돌 처리를 수행
            if(completeRatio > 0.5f)
            {
                Vector3 rayOrigin = character.transform.position; // 무조건 Bottom을 기준으로 해야 아래로 쏠 수 있음.
                Ray ray = new Ray(rayOrigin, Vector3.down);
                Debug.DrawRay(rayOrigin, Vector3.down, Color.blue);
                float collisiionDepth = skillData.SkillHitboxSize * SkillData.cm2m;
                int layerMask = SkillTargetToLayerMask(skillData.SkillCastingTarget);
                if (Physics.Raycast(ray, out RaycastHit characterHit, collisiionDepth, layerMask))
                {
                    hittedCharacters.Add(characterHit.transform.GetComponent<CharacterBase>());
                }
            }

            Vector3 nextPos = EvaluateParabola(initialPos, targetPos, 3, completeRatio); // 3은 임시 값 추후 높이 값으로 수정 필요
            character.transform.position = nextPos;
        }

        // 캐릭터 타격
        foreach (var hittedCharacter in hittedCharacters.Distinct())
        {
            float damageAmount = skillData.SkillDamage * character.Stat.Damage;

            hittedCharacter.TakeDamage(damageAmount);
        }

        // 초기화
        character.transform.position = targetPos;
        rigid.velocity = Vector3.zero;
        character.ColliderManager.SetActiveCollider(true, Define.ColliderType.PERSISTANCE);
        rigid.useGravity = true;
        Debug.Log("Stomp End");
    }

    // 포물선 운동을 하게 해주는 메소드
    // normalizedTime 값에 따라 start부터 end까지의 포물선 중 어느 위치에 있는지를 반환한다.
    private Vector3 EvaluateParabola(Vector3 start, Vector3 end, float height, float normalizedTime)
    {
        float quadratic = -4 * height * normalizedTime * normalizedTime + 4 * height * normalizedTime;

        Vector3 vec = Vector3.Lerp(start, end, normalizedTime);

        return new Vector3(vec.x, quadratic + start.y, start.z);
    }
}
