using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.TextCore.Text;


[CreateAssetMenu(fileName = "VerticalFallRunner", menuName = "ScriptableObjects/Skill/Runner/VerticalFallRunner", order = 1)]
public class VerticalFallRunner : SkillRunnerBase
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override IEnumerator SkillCoroutine(CharacterBase character)
    {
        // #*********** 초기화 ***********# // 
        // 사용 횟수 차감
        if (CurrentSkill is NormalSkill skill)
        {
            //skill.UseSkillCount();
        }

        if (skillData.SkillCastingTarget == Define.SkillTarget.MON)
        {
            TestSound.Instance.PlaySound("Skill2_Effect");
            TestSound.Instance.PlaySound("Skill2_Voice");
        }

        Rigidbody rigid = character.Rb;
        rigid.useGravity = false;

        Vector3 initialPos = character.transform.position;
         
        // 히트박스
        character.ColliderManager.SetActiveCollider(false, Define.ColliderType.PERSISTANCE);

        // 낙하
        float curTime = 0;
        float regenTime = skillData.SkillRegenTime * SkillData.Time2Second;

        List<CharacterBase> hittedCharacters = new List<CharacterBase>();

        // 바닥 체크
        Vector3 rayOrigin = character.transform.position;
        Ray ray = new Ray(rayOrigin, Vector3.down);

        Vector3 groundPos = Vector3.zero;
        float ySpeed = 0;
        if (Physics.Raycast(ray, out RaycastHit groundHit, 1000, 1 << 12 | 1 << 8)) // Ground 레이어 12 검출
        {
            groundPos = groundHit.point;

            float yGap = character.transform.position.y - groundPos.y;
            ySpeed = yGap / regenTime;
        }
        else
        {
            Debug.LogError("Failed to Detection for Ground");
        }

        // #*********** 스킬 행동 ***********# // 
        // 낙하 운동 시작
        while (curTime <= regenTime)
        {
            yield return null;

            curTime += Time.deltaTime;

            rayOrigin = character.transform.position; // 무조건 Bottom을 기준으로 해야 아래로 쏠 수 있음.

            //ray = new Ray(rayOrigin, Vector3.down);
            //Debug.DrawRay(rayOrigin, Vector3.down, Color.blue);

            //float collisiionDepth = skillData.SkillHitboxSize * SkillData.cm2m;
            //int layerMask = SkillTargetToLayerMask(skillData.SkillCastingTarget);
            //if (Physics.Raycast(ray, out RaycastHit characterHit, collisiionDepth, layerMask))
            //{
            //    hittedCharacters.Add(characterHit.transform.GetComponent<CharacterBase>());
            //    groundPos = character.transform.position;
            //    break;
            //}

            float yPos = character.transform.position.y;            // 현재 높이
            float curYSpeed = ySpeed * Time.deltaTime;              // 착지 속도
            character.Ani?.SetFloat("VerticalSpeed", -curYSpeed);   // 착지 애니를 위한 수치 변경
            Vector3 nextPos = new Vector3(initialPos.x, yPos - curYSpeed, initialPos.z);

            // 만약 움직였을 때 바닥에 닿는다면
            if(Physics.Linecast(character.transform.position, nextPos, 1 << 12))
            {
                // 즉시 낙하 종료
                break;
            }

            character.transform.position = nextPos;
        }

        // #*********** 후처리 ***********# // 
        // 캐릭터 타격
        //foreach (var hittedCharacter in hittedCharacters.Distinct())
        //{
        //    float damageAmount = skillData.SkillDamage * character.Stat.Damage;

        //    hittedCharacter.TakeDamage(damageAmount);
        //}

        Player player = character.GetComponent<Player>();
        int LeftDir = player.IsLeftDirection() ? -1 : 1;
        Vector3 originScale = player.HitPoint.localScale;

        player.HitPoint.localScale = new Vector3(3, 2, 1);
        Collider[] hittedCharacter = Physics.OverlapBox(player.HitPoint.position, player.HitPoint.localScale / 2, player.HitPoint.rotation, player.MonsterLayer | player.BossLayer);
        Collider[] hittedObj = Physics.OverlapBox(player.HitPoint.position, player.HitPoint.localScale / 2, player.HitPoint.rotation, 1 << 8);
        player.HitPoint.localScale = originScale;

        if (hittedCharacter.Length > 0)
        {
            TestSound.Instance.PlaySound("Skill2_Hit");
            foreach (var hitCharacter in hittedCharacter)
            {
                hitCharacter.GetComponent<CharacterBase>().TakeDamage(skillData.SkillDamage);
            }
        }

        if (hittedObj.Length > 0)
        {
            TestSound.Instance.PlaySound("Skill2_Hit");
            foreach (var hitObj in hittedObj)
            {
                hitObj.GetComponent<DestoryObj>().TakeDamage(skillData.SkillDamage);
            }
        }

        // 초기화
        character.transform.position = groundPos == Vector3.zero ? character.transform.position : groundPos;
        rigid.velocity = Vector3.zero;
        character.ColliderManager.SetActiveCollider(true, Define.ColliderType.PERSISTANCE);
        rigid.useGravity = true;

        Debug.Log("Vertical Fall End");
    }
}
