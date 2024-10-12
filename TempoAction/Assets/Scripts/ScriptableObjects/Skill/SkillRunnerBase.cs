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

    public virtual void Run(CharacterBase character, UnityAction OnEnded = null)
    {
        Initialize();
        character.StartCoroutine(WaitForSkillEnded(character, OnEnded));
    }
    
    /// <summary>
    /// 초기화 메소드. 
    /// 이펙트 등의 메소드를 초기화 하는 용도. 
    /// *//preDelayWFS를 사용하려면 반드시 오버라이드 이후 호출 필요//*
    /// </summary>
    public virtual void Initialize()
    {
        if (preDelayWFS == null)
        {
            preDelayWFS = new WaitForSeconds(skillData.SkillCastingTime * SkillData.Time2Second);
        }
    }

    /// <summary>
    /// 실제로 스킬을 사용하는 부분.
    /// 스킬을 사용했을 때 발생될 행동을 정의하는 코루틴
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

            // 이펙트가 살아 있는지 체크
            removingEffects.Clear();
            foreach (var effect in effects)
            {
                if (!effect.IsAlive(true)) { removingEffects.Add(effect); }
            }

            // 죽은 이펙트는 비활성화 및 더이상 검사하지 않도록 삭제
            foreach (var effect in removingEffects)
            {
                effect.gameObject.SetActive(false);
                effects.Remove(effect);
            }
        }
    }

    /// <summary>
    /// character의 위치에 effect를 활성화한다.
    /// </summary>
    /// <param name="character">대상 캐릭터</param>
    /// <param name="effect">대상 이펙트</param>
    protected void ActiveEffectToCharacter(CharacterBase character, GameObject effect)
    {
        if (character.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            effect.transform.position = character.transform.position + new Vector3(0, 1);
        }
        else
        {
            effect.transform.position = character.transform.position;
        }
        effect.SetActive(true);
    }

    /// <summary>
    /// array의 null체크, 길이 체크(0인지 아닌지), element의 null체크를 수행한다.
    /// </summary>
    /// <param name="array">대상 배열</param>
    /// <returns>각종 체크를 수행하여 통과하면 true</returns>
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
            // 무한반복이 아니고 자식 중 하나라도 살아 있다면
            if (!effect.main.loop && effect.IsAlive(true))
            {
                waitingEffects.Add(effect);
                continue;
            }

            // 비활성화
            effect.gameObject.SetActive(false);
        }

        // 삭제되지 않은 이펙트 대기 후 삭제
        character.StartCoroutine(WaitForEffectEnded(waitingEffects));
    }

    /// <summary>
    /// 충돌을 고려하여 Target Position(최종 위치)을 구한다.
    /// 충돌은 Wall Layer만 고려한다.
    /// </summary>
    /// <param name="initialPos">초기 위치</param>
    /// <param name="direction">캐릭터의 방향(좌우)</param>
    /// <param name="targetPos">현재 계산된 목표 위치</param>
    /// <param name="movingDistance">이동할 거리</param>
    /// <returns>(targetPos - initialPos) 벡터가, Wall에 충돌한다면 Wall에서 일정 거리 앞, 아니라면 원래 위치를 반환</returns>
    protected Vector3 GetTargetPosByCoillision(Vector3 initialPos, Vector3 direction, Vector3 targetPos, float movingDistance)
    {
        // 도착 지점 갱신 with Wall
        if (Physics.Raycast(new Ray(initialPos, direction), out RaycastHit wallHit, (targetPos - initialPos).magnitude, 1 << 13)) // 13은 Wall
        {
            movingDistance = (wallHit.distance - 0.6f) * 0.99f;
            return initialPos + direction * movingDistance;
        }

        return targetPos;
    }

    /// <summary>
    ///  SkillTarget에 해당하는 타겟 중 씬 내에서 유효한 타겟을 전부 가져온다.
    /// </summary>
    /// <param name="caster">스킬 시전자</param>
    /// <param name="target">스킬 대상</param>
    /// <returns>씬 내의 유효 스킬 대상을 전부 반환</returns>
    protected List<GameObject> GetTargets(CharacterBase caster, Define.SkillTarget target)
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
            case Define.SkillTarget.GROUND: // 미구현
                break;
            default:
                Debug.LogError("Invalid Target");
                break;
        }

        return targets;
    }

    /// <summary>
    /// objs 내의 요소 중에, target에 해당하는 GameObject만을 반환
    /// </summary>
    /// <param name="caster">스킬 시전자</param>
    /// <param name="target">스킬 대상</param>
    /// <param name="objs">검사할 GameObjects</param>
    /// <returns>target인 objs내의 요소 일체</returns>
    protected List<GameObject> GetExistingTargets(CharacterBase caster, Define.SkillTarget target, GameObject[] objs)
    {
        var targets = GetTargets(caster, target);
        var existingTargets = new List<GameObject>();

        foreach (var obj in objs)
        {
            // CharacterManager에 등록되지 않은 obj는 스킵
            // Ground의 경우도 받아온 Ground 리스트 안에 존재하지 않는 경우 판별할 예정
            if (!targets.Contains(obj)) { continue; }

            // 등록된 obj만 추가
            existingTargets.Add(obj);
        }

        return existingTargets;
    }

    /// <summary>
    /// Define.SkillTarget을 LayerMask로 변환하여 반환합니다.
    /// </summary>
    /// <param name="target">스킬 대상</param>
    /// <returns>대상의 LayerMask. 변환 불가한 경우 -1 반환</returns>
    protected int SkillTargetToLayerMask(Define.SkillTarget target)
    {
        int mask = -1;
        switch (target)
        {
            case Define.SkillTarget.PC:
                mask = 1 << 11;
                break;
            case Define.SkillTarget.MON:
                mask = 1 << 10;
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