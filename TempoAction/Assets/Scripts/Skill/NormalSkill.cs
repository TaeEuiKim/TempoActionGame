using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Experimental.Rendering;

[Serializable]
public class NormalSkill : SkillBase
{
    [field: SerializeField] public float cooldown { get; private set; } // seconds
    private const float cooldownMultiplier = 0.01f;

    private float curTime; // seconds

    // cooldown: 1/100 seconds
    public NormalSkill(float cooldown, int id, string name, Define.SkillColliderType hitboxType, float hitboxSize, float damage, Define.SkillEffectType effectType, float effectValue) : base(id, name, hitboxType, hitboxSize, damage, effectType, effectValue)
    {
        this.cooldown = cooldown * cooldownMultiplier;
        curTime = this.cooldown;
        OnSkillAttack.AddListener((SkillManager sm) => { Debug.Log("Invoke Normal Skill"); });
        OnSkillAttack.AddListener(SwordQuickDraw);
    }

    public virtual void UpdateTime(float deltaTime)
    {
        if(curTime > cooldown) { return; }

        curTime += deltaTime;
    }

    public bool IsCooldown() => cooldown > curTime;

    public override bool UseSkill(SkillManager sm) 
    {
        bool isRemove = false;
        if (IsCooldown()) { isRemove = true; }

        OnSkillAttack.Invoke(sm);

        curTime = 0;

        return isRemove;
    }

    private void SwordQuickDraw(SkillManager sm)
    {
        sm.StartCoroutine(SwordQuickDrawCoroutine(sm));   
    }

    public IEnumerator SwordQuickDrawCoroutine(SkillManager sm)
    {
        // 히트박스
        BoxCollider hitbox = sm.hitbox as BoxCollider;
        hitbox.enabled = false;
        sm.offingHitbox2.enabled = false;
        sm.offingHitbox.SetActive(false);
        var size = hitbox.size;
        size.x = skillHitboxSize * 0.01f;
        hitbox.size = size;
        sm.effectsParent.transform.eulerAngles = new Vector3(0, 180, 0) * (sm.target.localScale.x > 0 ? 1 : 0);
        sm.instiatedEffects[0].SetActive(true);


        // 선딜
        yield return new WaitForSeconds(25 / 100);

        // 돌진
        float curTime = 0;
        float movingDistance = skillEffectValue * 0.01f;
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

        sm.instiatedEffects[1].SetActive(true);
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
            monster.TakeDamage(skillDamage);
        }

        //yield return new WaitForEndOfFrame();

        sm.transform.position = targetPos;
        rigid.velocity = Vector3.zero;
        sm.offingHitbox.SetActive(true);
        sm.offingHitbox2.enabled = true;

        sm.instiatedEffects[2].SetActive(true);
        sm.instiatedEffects[1].SetActive(false);
        sm.instiatedEffects[0].SetActive(false);
        //hitbox.enabled = false;
        //rigid.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Debug.Log("QuickDraw End");

        yield return new WaitForSeconds(1f);

        sm.instiatedEffects[2].SetActive(false);
    }
}
