using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.TextCore.Text;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SwordQuickDrawRunner", menuName = "ScriptableObjects/Skill/Runner/SwordQuickDrawRunner", order = 1)]
public class SwordQuickDrawRunner : SkillRunnerBase
{
    [Header("����Ʈ�� ������ �� Left�� 0������")]
    public GameObject ReadyEffect;
    public GameObject[] DashEffect;
    public GameObject[] SwordEffect;

    private GameObject readyEffect;
    private GameObject[] dashEffect;
    private GameObject[] swordEffect;

    private GameObject effectParent;

    private WaitForSeconds preDelayWFS;

    public override void Initialize()
    {
        // ����Ʈ 
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

        // ��� �ð�
        if(preDelayWFS == null)
        {
            preDelayWFS = new WaitForSeconds(skillData.SkillCastingTime * SkillData.Time2Second);
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
        

        // �غ� ����Ʈ
        ActiveEffectToCharacter(character, ready);

        // ����
        yield return preDelayWFS;

        // ��Ʈ�ڽ�
        character.ColliderManager.SetActiveCollider(false, Define.ColliderType.PERSISTANCE);

        // ����
        float curTime = 0;
        float movingDistance = skillData.SkillEffectValue * SkillData.cm2m;
        float originalMovingDistance = movingDistance;
        float regenTime = skillData.SkillRegenTime * SkillData.Time2Second;
        Vector3 initialPos = character.transform.position;
        Vector3 direction = character.transform.right * (isLeftDir ? -1 : 1);
        Vector3 targetPos = initialPos + direction * movingDistance;
        List<Monster> hittedMonsters = new List<Monster>();
        Player hittedPlayer = new Player();

        // ���� ���� ���� with Wall
        if (Physics.Raycast(new Ray(initialPos, direction), out RaycastHit wallHit, (targetPos - initialPos).magnitude, 1 << 13)) // 13�� Wall
        {
            movingDistance = (wallHit.distance - 0.6f) * 0.99f;
            targetPos = initialPos + direction * movingDistance;
        }

        // ��� ����Ʈ ����
        ActiveEffectToCharacter(character, dash);

        // ��� ����
        while ((character.transform.position - targetPos).magnitude > 0.1f && curTime <= regenTime)
        {
            yield return null;

            curTime += Time.deltaTime * 3f;
            if (skillData.SkillCastingTarget == Define.SkillTarget.MON)
            {
                Ray ray = new Ray(character.transform.position, direction.normalized);
                Debug.DrawRay(character.transform.position + new Vector3(0, 1f), direction.normalized, Color.blue);
                float collisiionDepth = skillData.SkillHitboxSize * SkillData.cm2m;

                if (Physics.Raycast(ray, out RaycastHit monsterHit, collisiionDepth, 1 << 10))
                {
                    hittedMonsters.Add(monsterHit.transform.GetComponent<Monster>());
                }
            }
            else if (skillData.SkillCastingTarget == Define.SkillTarget.PC)
            {
                Ray ray = new Ray(character.transform.position + new Vector3(0, 1f), direction.normalized);
                Debug.DrawRay(character.transform.position + new Vector3(0, 1f), direction.normalized, Color.blue);
                float collisiionDepth = skillData.SkillHitboxSize * SkillData.cm2m;

                if (Physics.Raycast(ray, out RaycastHit playerHit, collisiionDepth, 1 << 11))
                {
                    hittedPlayer = playerHit.transform.GetComponent<Player>();
                }
            }

            character.transform.position = Vector3.Lerp(initialPos, targetPos, curTime / regenTime * (originalMovingDistance / movingDistance));
        }

        // ���� Ÿ��
        foreach (Monster monster in hittedMonsters.Distinct())
        {
            float damageAmount = skillData.SkillDamage * 1; // 1��� ���ݷ� ���� ��

            monster.TakeDamage(damageAmount);
        }

        // �÷��̾� Ÿ��
        if (hittedPlayer != null)
        {
            hittedPlayer.TakeDamage(skillData.SkillDamage, true);
        }

        // �ʱ�ȭ
        character.transform.position = targetPos;
        rigid.velocity = Vector3.zero;
        character.ColliderManager.SetActiveCollider(true, Define.ColliderType.PERSISTANCE);

        // ����Ʈ ���� �� �� ����Ʈ ���
        ActiveEffectToCharacter(character, sword);
        dash.SetActive(false);
        ready.SetActive(false);
        rigid.useGravity = true;
        Debug.Log("QuickDraw End");

        yield return new WaitForSeconds(0.4f); // �ߵ��� ����Ʈ �����⸦ ��ٸ�

        sword.SetActive(false);
    }
}