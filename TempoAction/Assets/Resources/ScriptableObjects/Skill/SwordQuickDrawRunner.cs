using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public override void Run()
    {
        Initialize();
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

    /*private void SwordQuickDraw(ISkillManager sm)
    {
        var manager = (PlayerSkillManager)sm;

        manager.StartCoroutine(SwordQuickDrawCoroutine(manager));
    }

    public IEnumerator SwordQuickDrawCoroutine(ISkillManager sm)
    {
        GameObject ready = readyEffect;
        GameObject dash = dashEffect[sm.target.localScale.x < 0 ? 0 : 1];
        GameObject sword = swordEffect[sm.target.localScale.x < 0 ? 0 : 1];

        // 히트박스
        BoxCollider hitbox = sm.hitbox as BoxCollider;
        hitbox.enabled = false;
        sm.offingHitbox2.enabled = false;
        sm.offingHitbox.SetActive(false);
        var size = hitbox.size;
        size.x = skillData.SkillHitboxSize * 0.01f;
        hitbox.size = size;
        //sm.effectsParent.transform.eulerAngles = new Vector3(0, 180, 0) * ;
        ready.SetActive(true);


        // 선딜
        yield return new WaitForSeconds(25 / 100);

        // 돌진
        float curTime = 0;
        float movingDistance = skillData.SkillEffectValue * 0.01f;
        float originalMovingDistance = movingDistance;
        float regenTime = 0.5f;
        Vector3 initialPos = sm.transform.position;
        Vector3 direction = sm.transform.right * sm.target.localScale.x;
        Vector3 targetPos = initialPos + direction * movingDistance;
        List<Monster> hittedMonsters = new List<Monster>();
        Rigidbody rigid = sm.transform.GetComponent<Rigidbody>();
        //rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        if (Physics.Raycast(new Ray(initialPos, direction), out RaycastHit wallHit, (targetPos - initialPos).magnitude, 1 << 13))
        {
            movingDistance = (wallHit.distance - 0.6f) * 0.99f;
            targetPos = initialPos + direction * movingDistance;
        }

        effect2.SetActive(true);
        while ((sm.transform.position - targetPos).magnitude > 0.1f && curTime <= regenTime)
        {
            yield return new WaitForEndOfFrame();

            curTime += Time.deltaTime;

            Ray ray = new Ray(sm.transform.position, direction.normalized);

            if (Physics.Raycast(ray, out RaycastHit monsterHit, size.x, 1 << 10))
            {
                hittedMonsters.Add(monsterHit.transform.GetComponent<Monster>());
            }

            //rigid.MovePosition(Vector3.Lerp(initialPos, targetPos, curTime / regenTime + Vector3.up);
            sm.transform.position = Vector3.Lerp(initialPos, targetPos, (curTime / regenTime * (originalMovingDistance / movingDistance)));
            //rigid.velocity = direction * (movingDistance / regenTime);
        }

        foreach (Monster monster in hittedMonsters.Distinct())
        {
            float damageAmount = skillData.SkillDamage * 1; // 1대신 공격력 들어가야 함

            monster.TakeDamage(damageAmount);
        }

        //yield return new WaitForEndOfFrame();

        sm.transform.position = targetPos;
        rigid.velocity = Vector3.zero;
        sm.offingHitbox.SetActive(true);
        sm.offingHitbox2.enabled = true;

        effect3.SetActive(true);
        effect2.SetActive(false);
        ready.SetActive(false);
        //hitbox.enabled = false;
        //rigid.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Debug.Log("QuickDraw End");

        yield return new WaitForSeconds(1f);

        effect3.SetActive(false);
    }*/
}