using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

[Serializable]
public class NormalSkill : SkillBase<PlayerNormalSkillData>, ICooldownSkill
{
    private static float cooldownMultiplier = 0.01f;

    private float curTime; // seconds

    // cooldown: 1/100 seconds
    public NormalSkill(PlayerNormalSkillData skillData) : base(skillData)
    {
        curTime = skillData.SkillCooldown;
        OnSkillAttack.AddListener((ISkillManager sm) => { Debug.Log("Invoke Normal Skill(Player)"); });
        OnSkillAttack.AddListener(SwordQuickDraw);
    }

    public virtual void UpdateTime(float deltaTime)
    {
        if(curTime > SkillData.SkillCooldown) { return; }

        curTime += deltaTime;
    }

    public bool IsCooldown() => SkillData.SkillCooldown > curTime;

    public override bool UseSkill(ISkillManager sm) 
    {
        bool isRemove = false;
        if (IsCooldown()) { isRemove = true; }

        OnSkillAttack.Invoke(sm);

        curTime = 0;

        return isRemove;
    }

    private void SwordQuickDraw(ISkillManager sm)
    {
        var manager = (PlayerSkillManager)sm;

        manager.StartCoroutine(SwordQuickDrawCoroutine(manager));   
    }

    public IEnumerator SwordQuickDrawCoroutine(PlayerSkillManager sm)
    {
        GameObject effect1 = sm.instiatedEffects[sm.target.localScale.x < 0 ? 0 : 1];
        GameObject effect2 = sm.instiatedEffects[2];
        GameObject effect3 = sm.instiatedEffects[sm.target.localScale.x < 0 ? 3 : 4];
        
        // 히트박스
        BoxCollider hitbox = sm.hitbox as BoxCollider;
        hitbox.enabled = false;
        sm.offingHitbox2.enabled = false;
        sm.offingHitbox.SetActive(false);
        var size = hitbox.size;
        size.x = SkillData.SkillHitboxSize * 0.01f;
        hitbox.size = size;
        //sm.effectsParent.transform.eulerAngles = new Vector3(0, 180, 0) * ;
        effect1.SetActive(true);


        // 선딜
        yield return new WaitForSeconds(25 / 100);

        // 돌진
        float curTime = 0;
        float movingDistance = SkillData.SkillEffectValue * 0.01f;
        float originalMovingDistance = movingDistance;
        float regenTime = 0.5f;
        Vector3 initialPos = sm.transform.position;
        Vector3 direction = sm.transform.right * sm.target.localScale.x;
        Vector3 targetPos = initialPos + direction * movingDistance;
        List<Monster> hittedMonsters = new List<Monster>();
        Rigidbody rigid = sm.transform.GetComponent<Rigidbody>();
        //rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        if (Physics.Raycast(new Ray(initialPos, direction), out RaycastHit wallHit,(targetPos - initialPos).magnitude, 1 << 13))
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

            if(Physics.Raycast(ray, out RaycastHit monsterHit, size.x, 1 << 10))
            {
                hittedMonsters.Add(monsterHit.transform.GetComponent<Monster>());
            }

            //rigid.MovePosition(Vector3.Lerp(initialPos, targetPos, curTime / regenTime + Vector3.up);
            sm.transform.position = Vector3.Lerp(initialPos, targetPos, (curTime / regenTime * (originalMovingDistance / movingDistance))) ;
            //rigid.velocity = direction * (movingDistance / regenTime);
        }
        
        foreach(Monster monster in hittedMonsters.Distinct())
        {
            float damageAmount = SkillData.SkillDamage * 1; // 1대신 공격력 들어가야 함

            monster.TakeDamage(damageAmount);
        }

        //yield return new WaitForEndOfFrame();

        sm.transform.position = targetPos;
        rigid.velocity = Vector3.zero;
        sm.offingHitbox.SetActive(true);
        sm.offingHitbox2.enabled = true;

        effect3.SetActive(true);
        effect2.SetActive(false);
        effect1.SetActive(false);
        //hitbox.enabled = false;
        //rigid.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Debug.Log("QuickDraw End");

        yield return new WaitForSeconds(1f);

        effect3.SetActive(false);
    }
}
