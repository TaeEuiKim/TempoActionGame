using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.TextCore.Text;
using UnityEngine.Events;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "SwordQuickDrawRunner", menuName = "ScriptableObjects/Skill/Runner/SwordQuickDrawRunner", order = 1)]
public class SwordQuickDrawRunner : SkillRunnerBase
{
    [Header("이펙트가 복수인 건 Left가 0번으로")]
    public GameObject ReadyEffect;
    public GameObject[] DashEffect;
    public GameObject[] SwordEffect;

    private GameObject readyEffect;
    private GameObject[] dashEffect;
    private GameObject[] swordEffect;

    private GameObject effectParent;

    public override void Initialize()
    {
        base.Initialize();

        // 이펙트 
        if(effectParent == null)
        {
            effectParent = new GameObject("Effects");
            effectParent.transform.position = Vector3.zero;
        }
        if(readyEffect == null)
        {
            readyEffect= Instantiate(ReadyEffect, effectParent.transform);
            readyEffect.SetActive(false);

            //managedEffects.Add(readyEffect.GetComponent<ParticleSystem>());
        }
        if (!IsValidArray(dashEffect))
        {
            dashEffect = new GameObject[DashEffect.Length];
            dashEffect[0] = Instantiate(DashEffect[0], effectParent.transform);
            dashEffect[1] = Instantiate(DashEffect[1], effectParent.transform);
            dashEffect[0].SetActive(false);
            dashEffect[1].SetActive(false);

            //managedEffects.Add(dashEffect[0].GetComponent<ParticleSystem>());
            //managedEffects.Add(dashEffect[1].GetComponent<ParticleSystem>());
        }
        if (!IsValidArray(swordEffect))
        {
            swordEffect = new GameObject[SwordEffect.Length];
            swordEffect[0] = Instantiate(SwordEffect[0], effectParent.transform);
            swordEffect[1] = Instantiate(SwordEffect[1], effectParent.transform);
            swordEffect[0].SetActive(false);
            swordEffect[1].SetActive(false);

            //managedEffects.Add(swordEffect[0].GetComponent<ParticleSystem>());
            //managedEffects.Add(swordEffect[1].GetComponent<ParticleSystem>());
        }
    }

    public override IEnumerator SkillCoroutine(CharacterBase character)
    {
        bool isLeftDir = character.IsLeftDirection();

        GameObject ready = readyEffect;
        GameObject dash = dashEffect[isLeftDir ? 0 : 1];
        GameObject sword = swordEffect[isLeftDir ? 0 : 1];
        Rigidbody rigid = character.Rb;
        rigid.useGravity = false;

        // 히트박스
        character.ColliderManager.SetActiveCollider(false, Define.ColliderType.PERSISTANCE);
        
        // 준비 이펙트
        ActiveEffectToCharacter(character, ready);

        // 선딜
        yield return preDelayWFS;

        // 돌진
        float curTime = 0;
        float movingDistance = skillData.SkillEffectValue * SkillData.cm2m;
        float originalMovingDistance = movingDistance;
        float regenTime = skillData.SkillRegenTime * SkillData.Time2Second;
        Vector3 initialPos = character.transform.position;
        Vector3 direction = character.transform.right * (isLeftDir ? -1 : 1);
        Vector3 targetPos = initialPos + direction * movingDistance;
        List<CharacterBase> hittedCharacters = new List<CharacterBase>();

        // 도착 지점 갱신 with Wall
        targetPos = GetTargetPosByCoillision(initialPos, direction, targetPos, movingDistance);

        // 돌진 이펙트 시작
        ActiveEffectToCharacter(character, dash);

        // 돌진 시작
        while ((character.transform.position - targetPos).magnitude > 0.1f && curTime <= regenTime)
        {
            yield return null;

            curTime += Time.deltaTime * 3f;

            Vector3 rayOrigin = character.GetRayOrigin();
            Ray ray = new Ray(rayOrigin, direction.normalized);
            Debug.DrawRay(rayOrigin, direction.normalized, Color.blue);
            float collisiionDepth = skillData.SkillHitboxSize * SkillData.cm2m;
            int layerMask = SkillTargetToLayerMask(skillData.SkillCastingTarget);
            if (Physics.Raycast(ray, out RaycastHit monsterHit, collisiionDepth, layerMask))
            {
                hittedCharacters.Add(monsterHit.transform.GetComponent<CharacterBase>());
            }

            character.transform.position = Vector3.Lerp(initialPos, targetPos, curTime / regenTime * (originalMovingDistance / movingDistance));
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

        yield return new WaitForSeconds(0.2f);
        // 이펙트 종료 및 검 이펙트 재생
        //ActiveEffectToCharacter(character, sword);
        dash.SetActive(false);
        ready.SetActive(false);
        rigid.useGravity = true;
        Debug.Log("QuickDraw End");

        yield return new WaitForSeconds(0.4f); // 발도술 이펙트 끝나기를 기다림

        //sword.SetActive(false);
    }
}