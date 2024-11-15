using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class SkillRunnerBase : ScriptableObject
{
    public SkillData skillData;
    protected List<ParticleSystem> managedEffects = new List<ParticleSystem>();
    protected WaitForSeconds preDelayWFS;
    protected ISkillRoot CurrentSkill { get; private set; }

    public virtual void Run(ISkillRoot skill, CharacterBase character, UnityAction OnEnded = null)
    {
        Initialize();
        CurrentSkill = skill;
        character.StartCoroutine(WaitForSkillEnded(character, OnEnded));
    }
    
    /// <summary>
    /// 초기화 함수 
    /// 이펙트 등을 초기화 하는 부분. 
    /// preDelayWFS는 자동으로 정의하므로, 이 값을 그대로 사용하려면 해당 메소드 무시하지 말 것.
    /// </summary>
    public virtual void Initialize()
    {
        if (preDelayWFS == null)
        {
            preDelayWFS = new WaitForSeconds(skillData.SkillCastingTime * SkillData.Time2Second);
        }
    }

    /// <summary>
    /// 캐릭터의 스킬 처리 메소드
    /// 스킬을 사용하는 동안 발생하는 행동을 정의
    /// </summary>
    /// <param name="character">스킬 사용자(캐릭터)</param>
    /// <returns></returns>
    public abstract IEnumerator SkillCoroutine(CharacterBase character);

    private IEnumerator WaitForSkillEnded(CharacterBase character, UnityAction OnFinished)
    {
        while (true)
        {
            yield return SkillCoroutine(character);

            OnFinished?.Invoke();
            CleanupEffects(character);
            break;
        }

        Debug.Log("Success");
    }

    private IEnumerator WaitForEffectEnded(List<ParticleSystem> effects)
    {
        List<ParticleSystem> removingEffects = new List<ParticleSystem>();
        while (effects.Count > 0)
        {
            yield return null;

            // ����Ʈ�� ��� �ִ��� üũ
            removingEffects.Clear();
            foreach (var effect in effects)
            {
                if (!effect.IsAlive(true)) { removingEffects.Add(effect); }
            }

            // ���� ����Ʈ�� ��Ȱ��ȭ �� ���̻� �˻����� �ʵ��� ����
            foreach (var effect in removingEffects)
            {
                effect.gameObject.SetActive(false);
                effects.Remove(effect);
            }
        }
    }

    /// <summary>
    /// character의 위치에서 effect를 활성화.
    /// </summary>
    /// <param name="character">대상 캐릭터</param>
    /// <param name="effect">적용할 이펙트</param>
    protected void ActiveEffectToCharacter(CharacterBase character, GameObject effect)
    {
        if (character.Stat.Hp <= 0)
        {
            return;
        }

        if (character.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            effect.transform.position = character.transform.position + new Vector3(0, 1);
        }
        else
        {
            effect.transform.position = character.transform.position + new Vector3(0, 0.5f);
        }
        effect.SetActive(true);
    }

    protected void ActiveEffectToCharacter(CharacterBase character, GameObject effect, Vector3 targetPos)
    {
        effect.transform.position = targetPos;
        effect.SetActive(true);
    }

    /// <summary>
    /// array의 null체크, 길이 체크(0이 아닌지), element의 null체크를 수행.
    /// </summary>
    /// <param name="array">대상 배열</param>
    /// <returns>검사 통과 시 true</returns>
    protected bool IsValidArray(GameObject[] array)
    {
        return array != null && array.Length > 0 && !Array.Exists(array, (value) => value == null);
    }

    private void CleanupEffects(CharacterBase character)
    {
        managedEffects = managedEffects.Where((effect) => effect != null).Distinct().ToList();

        List<ParticleSystem> waitingEffects = new List<ParticleSystem>();
        foreach (ParticleSystem effect in managedEffects)
        {
            // ���ѹݺ��� �ƴϰ� �ڽ� �� �ϳ��� ��� �ִٸ�
            if(!effect.main.loop && effect.IsAlive(true))
            {
                waitingEffects.Add(effect);
                continue;
            }

            // ��Ȱ��ȭ
            effect.gameObject.SetActive(false);
        }

        // �������� ���� ����Ʈ ��� �� ����
        character.StartCoroutine(WaitForEffectEnded(waitingEffects));
    }

    /// <summary>
    /// 충돌을 검사하여 Target Position(목표 위치) 계산.
    /// 충돌 대상은 Wall Layer로 한정
    /// </summary>
    /// <param name="initialPos">초기 위치</param>
    /// <param name="direction">캐릭터의 방향(좌우)</param>
    /// <param name="targetPos">계산된 목표 위치</param>
    /// <param name="distanceToWall">벽으로부터 떨어진 거리</param>
    /// <returns>(targetPos - initialPos) (+ distanceToWall) 벡터가, Wall과 충돌한다면, Wall까지의 거리 벡터, 아니라면 목표 위치 반환</returns>
    protected Vector3 GetTargetPosByCoillision(Vector3 initialPos, Vector3 direction, Vector3 targetPos, int layer, float distanceToWall = 0.6f)
    {
        // ���� ���� ���� with Wall
        if (Physics.Raycast(new Ray(initialPos, direction), out RaycastHit wallHit, (targetPos - initialPos).magnitude + distanceToWall, layer)) // 13은 Wall
        {
            var wallGap = wallHit.point + wallHit.normal * distanceToWall;
            return wallGap;
        }

        return targetPos;
    }

    /// <summary>
    ///  SkillTarget에 해당하는 타겟 중, 유효한 타겟만을 반환
    /// </summary>
    /// <param name="caster">스킬 시전자</param>
    /// <param name="target">스킬 대상</param>
    /// <returns>유효 타겟 리스트</returns>
    protected List<GameObject> GetTargets(Define.SkillTarget target, CharacterBase caster = null)
    {
        List<GameObject> targets = new List<GameObject>();
        int mask = SkillTargetToLayerMask(target);
        switch (target)
        {
            case Define.SkillTarget.NONE:
                break;
            case Define.SkillTarget.SELF:
                targets.Add(caster.gameObject);
                break;
            case Define.SkillTarget.PC:
            case Define.SkillTarget.MON:
            case Define.SkillTarget.ALL:
                targets = CharacterManager.Instance.GetCharacter(mask);
                break;
            case Define.SkillTarget.GROUND: // 미개발
                break;
            default:
                Debug.LogError("Invalid Target");
                break;
        }

        return targets;
    }

    /// <summary>
    /// objs 목록 중, target에 해당하는 GameObject만을 반환
    /// </summary>
    /// <param name="caster">스킬 시전자</param>
    /// <param name="target">스킬 대상</param>
    /// <param name="objs">검사할 GameObjects</param>
    /// <returns>objs 중, target에 해당하는 오브젝트</returns>
    protected List<GameObject> GetExistingTargets(CharacterBase caster, Define.SkillTarget target, GameObject[] objs)
    {
        var targets = GetTargets(target, caster);
        var existingTargets = new List<GameObject>();

        foreach (var obj in objs)
        {
            // CharacterManager에 등록된 대상 obj만을 처리
            //  Ground는 현재 작업되지 않음.
            if (!targets.Contains(obj)) { continue; }

            // 반환할 오브젝트에 추가
            existingTargets.Add(obj);
        }

        return existingTargets;
    }

    /// <summary>
    /// Define.SkillTarget을 LayerMask로 변환
    /// </summary>
    /// <param name="target">스킬 대상</param>
    /// <returns>변환된 LayerMask. 변환 불가 시 -1 반환</returns>
    protected int SkillTargetToLayerMask(Define.SkillTarget target)
    {
        int mask = -1;
        switch (target)
        {
            case Define.SkillTarget.PC:
                mask = 1 << 11;
                break;
            case Define.SkillTarget.MON:
                mask = 1 << 10 | 1 << 9;
                break;
            case Define.SkillTarget.GROUND:
                mask = 1 << 12;
                break;
            case Define.SkillTarget.ALL:
                mask = SkillTargetToLayerMask(Define.SkillTarget.PC) | SkillTargetToLayerMask(Define.SkillTarget.MON);
                break;
        }

        return mask;
    }
}