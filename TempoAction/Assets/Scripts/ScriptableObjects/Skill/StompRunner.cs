using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "StompRunner", menuName = "ScriptableObjects/Skill/Runner/StompRunner", order = 1)]
public class StompRunner : SkillRunnerBase
{
    public override IEnumerator SkillCoroutine(CharacterBase character)
    {
        Rigidbody rigid = character.Rb;
        rigid.useGravity = false;

        float movingDistance = skillData.SkillEffectValue * SkillData.cm2m;

        var targets = GetTargets(skillData.SkillCastingTarget, character);

        // 선딜
        yield return preDelayWFS;

        Vector3 initialPos = character.transform.position;
        Vector3 targetPos = targets[0].transform.position;
        Vector3 direction = (targetPos - initialPos);
        direction.y = 0f;
        direction.Normalize();

        // 점프 및 낙하
        float curTime = 0;
        float regenTime = skillData.SkillRegenTime * SkillData.Time2Second;

        List<CharacterBase> hittedCharacters = new List<CharacterBase>();

        // 콜라이더 사이즈 가져오기
        float halfColliderSize = character.ColliderManager.GetHalfSizeForMain(Vector3.right);
        float colliderSizeForTarget = 0;
        var target = GetCharacter(targets[0]);
        if(target != null)
        {
            colliderSizeForTarget = target.ColliderManager.GetHalfSizeForMain(Vector3.right) * 2f;
        }

        // 히트박스
        character.ColliderManager.SetActiveCollider(false, Define.ColliderType.PERSISTANCE);

        // 목표 위치 계산
        targetPos = GetTargetPosByCoillision(initialPos, direction, targetPos, 1 << 13, halfColliderSize + colliderSizeForTarget);

        if (character.transform.position.x - targets[0].transform.position.x > 0)
        {
            character.CharacterModel.localScale = new Vector3(-1, 1, 1);
        }
        else if (character.transform.position.x - targets[0].transform.position.x < 0)
        {
            character.CharacterModel.localScale = new Vector3(1, 1, 1);
        }

        TestSound.Instance.PlaySound("NormalMonster2_Skill");

        // 포물선 운동 시작
        while (curTime <= regenTime)
        {
            yield return null;

            curTime += Time.deltaTime;
            float completeRatio = curTime / regenTime;

            /// 최고 높이까지 도달한 이후에 충돌 처리를 수행
            //if(completeRatio > 0.5f)
            //{
            //    Vector3 rayOrigin = character.transform.position; // 무조건 Bottom을 기준으로 해야 아래로 쏠 수 있음.
            //    Ray ray = new Ray(rayOrigin, Vector3.down);
            //    Debug.DrawRay(rayOrigin, Vector3.down, Color.blue);
            //    float collisiionDepth = skillData.SkillHitboxSize * SkillData.cm2m;
            //    int layerMask = SkillTargetToLayerMask(skillData.SkillCastingTarget);
            //    if (Physics.Raycast(ray, out RaycastHit characterHit, collisiionDepth, layerMask))
            //    {
            //        hittedCharacters.Add(characterHit.transform.GetComponent<CharacterBase>());
            //    }
            //}

            Vector3 nextPos = EvaluateParabola(initialPos, targetPos, 3, completeRatio); // 3은 임시 값 추후 높이 값으로 수정 필요
            character.transform.position = nextPos;
        }

        yield return new WaitForSeconds(0.1f);

        NormalMonster monster = character.GetComponent<NormalMonster>();
        Vector3 originScale = monster.HitPoint.localScale;

        monster.HitPoint.localScale = new Vector3(3, 3, 1);
        Collider[] hittedCharacter = Physics.OverlapBox(monster.HitPoint.position, monster.HitPoint.localScale / 2, monster.HitPoint.rotation, monster.PlayerLayer);
        monster.HitPoint.localScale = originScale;

        foreach (var hitCharacter in hittedCharacter)
        {
            hitCharacter.GetComponent<CharacterBase>().TakeDamage(skillData.SkillDamage);
        }

        // 캐릭터 타격
        //foreach (var hittedCharacter in hittedCharacters.Distinct())
        //{
        //    float damageAmount = skillData.SkillDamage * character.Stat.Damage;

        //    hittedCharacter.TakeDamage(damageAmount);
        //}

        var targetsInRanged = Physics.SphereCastAll(character.transform.position, 5, Vector3.up, 0, SkillTargetToLayerMask(skillData.SkillCastingTarget));
        TestSound.Instance.PlaySound("NormalMonster2_AttackHit");

        GameObject effect = ObjectPool.Instance.Spawn("P_BalkoongMonsterSkill", 1f);
        effect.transform.position = character.transform.position + new Vector3(0, 0.3f);

        foreach (var targetInReanged in targetsInRanged)
        {
            var targetCharacter = targetInReanged.transform.GetComponent<CharacterBase>();
            if (targetCharacter != null)
            { 
                KnockbackTarget(targetCharacter, character, 5);
            }
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

    private CharacterBase GetCharacter(GameObject obj)
    {
        var character = obj.GetComponent<CharacterBase>();
        if (character == null)
        {
            Debug.LogWarning("This Object has not Character Base");
        }

        return character;
    }

    private void KnockbackTarget(CharacterBase target, CharacterBase self, float power)
    {
        Vector3 selfToTarget = target.transform.position - self.transform.position;
        selfToTarget.y = 0;

        if(selfToTarget == Vector3.zero)
        {
            selfToTarget = self.transform.right * (self.IsLeftDirection() ? -1 : 1);
        }

        Vector3 direction = selfToTarget.normalized;
        Vector3 velocity = direction * power;
        velocity.y = target.Rb.velocity.y;
        target.Rb.velocity = velocity;
    }
}
