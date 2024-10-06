using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.TextCore.Text;

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

    public override void Run(CharacterBase character)
    {
        Initialize();
        character.StartCoroutine(SwordQuickDrawCoroutine(character));
    }

    public void Initialize()
    {
        if(effectParent == null)
        {
            effectParent = new GameObject("Effects");
            effectParent.transform.position = Vector3.zero;
        }
        if(readyEffect == null)
        {
            readyEffect= Instantiate(ReadyEffect, effectParent.transform);
        }
        if (dashEffect == null)
        {
            dashEffect[0] = Instantiate(DashEffect[0], effectParent.transform);
            dashEffect[1] = Instantiate(DashEffect[1], effectParent.transform);
        }
        if (swordEffect == null)
        {
            swordEffect[0] = Instantiate(SwordEffect[0], effectParent.transform);
            swordEffect[1] = Instantiate(SwordEffect[1], effectParent.transform);
        }
    }

    public IEnumerator SwordQuickDrawCoroutine(CharacterBase character)
    {
        bool isLeftDir = character.IsLeftDirection();

        GameObject ready = readyEffect;
        GameObject dash = dashEffect[isLeftDir ? 0 : 1];
        GameObject sword = swordEffect[isLeftDir ? 0 : 1];

        // 히트박스
        character.ColliderManager.SetActiveCollider(false, Define.ColliderType.PERSISTANCE);
        ready.SetActive(true);

        // 선딜
        yield return new WaitForSeconds(skillData.SkillCastingTime * SkillData.Time2Second);

        // 돌진
        float curTime = 0;
        float movingDistance = skillData.SkillEffectValue * SkillData.cm2m;
        float originalMovingDistance = movingDistance;
        float regenTime = skillData.SkillRegenTime * SkillData.Time2Second;
        Vector3 initialPos = character.transform.position;
        Vector3 direction = character.transform.right * (isLeftDir ? -1 : 1);
        Vector3 targetPos = initialPos + direction * movingDistance;
        List<Monster> hittedMonsters = new List<Monster>();
        Rigidbody rigid = character.Rb;

        // 도착 지점 갱신 with Wall
        if (Physics.Raycast(new Ray(initialPos, direction), out RaycastHit wallHit, (targetPos - initialPos).magnitude, 1 << 13)) // 13은 Wall
        {
            movingDistance = (wallHit.distance - 0.6f) * 0.99f;
            targetPos = initialPos + direction * movingDistance;
        }

        // 대시 이펙트 시작
        dash.SetActive(true);

        // 대시 시작
        while ((character.transform.position - targetPos).magnitude > 0.1f && curTime <= regenTime)
        {
            yield return new WaitForEndOfFrame();

            curTime += Time.deltaTime;

            Ray ray = new Ray(character.transform.position, direction.normalized);
            float collisiionDepth = skillData.SkillHitboxSize * SkillData.cm2m;

            if (Physics.Raycast(ray, out RaycastHit monsterHit, collisiionDepth, 1 << 10))
            {
                hittedMonsters.Add(monsterHit.transform.GetComponent<Monster>());
            }

            character.transform.position = Vector3.Lerp(initialPos, targetPos, curTime / regenTime * (originalMovingDistance / movingDistance));
        }

        // 몬스터 타격
        foreach (Monster monster in hittedMonsters.Distinct())
        {
            float damageAmount = skillData.SkillDamage * 1; // 1대신 공격력 들어가야 함

            monster.TakeDamage(damageAmount);
        }

        // 초기화
        character.transform.position = targetPos;
        rigid.velocity = Vector3.zero;
        character.ColliderManager.SetActiveCollider(true, Define.ColliderType.PERSISTANCE);

        // 이펙트 종료 및 검 이펙트 재생
        sword.SetActive(true);
        dash.SetActive(false);
        ready.SetActive(false);
        Debug.Log("QuickDraw End");

        yield return new WaitForSeconds(1f);

        // 검 이펙트 종료
        sword.SetActive(false);
    }
}