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
    /// �ʱ�ȭ �޼ҵ�. 
    /// ����Ʈ ���� �޼ҵ带 �ʱ�ȭ �ϴ� �뵵. 
    /// *//preDelayWFS�� ����Ϸ��� �ݵ�� �������̵� ���� ȣ�� �ʿ�//*
    /// </summary>
    public virtual void Initialize()
    {
        if (preDelayWFS == null)
        {
            preDelayWFS = new WaitForSeconds(skillData.SkillCastingTime * SkillData.Time2Second);
        }
    }

    /// <summary>
    /// ������ ��ų�� ����ϴ� �κ�.
    /// ��ų�� ������� �� �߻��� �ൿ�� �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <param name="character">��ų �����(ĳ����)</param>
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
    /// character�� ��ġ�� effect�� Ȱ��ȭ�Ѵ�.
    /// </summary>
    /// <param name="character">��� ĳ����</param>
    /// <param name="effect">��� ����Ʈ</param>
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
    /// array�� nullüũ, ���� üũ(0���� �ƴ���), element�� nullüũ�� �����Ѵ�.
    /// </summary>
    /// <param name="array">��� �迭</param>
    /// <returns>���� üũ�� �����Ͽ� ����ϸ� true</returns>
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
    /// �浹�� �����Ͽ� Target Position(���� ��ġ)�� ���Ѵ�.
    /// �浹�� Wall Layer�� �����Ѵ�.
    /// </summary>
    /// <param name="initialPos">�ʱ� ��ġ</param>
    /// <param name="direction">ĳ������ ����(�¿�)</param>
    /// <param name="targetPos">���� ���� ��ǥ ��ġ</param>
    /// <param name="movingDistance">�̵��� �Ÿ�</param>
    /// <returns>(targetPos - initialPos) ���Ͱ�, Wall�� �浹�Ѵٸ� Wall���� ���� �Ÿ� ��, �ƴ϶�� ���� ��ġ�� ��ȯ</returns>
    protected Vector3 GetTargetPosByCoillision(Vector3 initialPos, Vector3 direction, Vector3 targetPos, float movingDistance)
    {
        // ���� ���� ���� with Wall
        if (Physics.Raycast(new Ray(initialPos, direction), out RaycastHit wallHit, (targetPos - initialPos).magnitude, 1 << 13)) // 13�� Wall
        {
            movingDistance = (wallHit.distance - 0.6f) * 0.99f;
            return initialPos + direction * movingDistance;
        }

        return targetPos;
    }

    /// <summary>
    ///  SkillTarget�� �ش��ϴ� Ÿ�� �� �� ������ ��ȿ�� Ÿ���� ���� �����´�.
    /// </summary>
    /// <param name="caster">��ų ������</param>
    /// <param name="target">��ų ���</param>
    /// <returns>�� ���� ��ȿ ��ų ����� ���� ��ȯ</returns>
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
            case Define.SkillTarget.GROUND: // �̱���
                break;
            default:
                Debug.LogError("Invalid Target");
                break;
        }

        return targets;
    }

    /// <summary>
    /// objs ���� ��� �߿�, target�� �ش��ϴ� GameObject���� ��ȯ
    /// </summary>
    /// <param name="caster">��ų ������</param>
    /// <param name="target">��ų ���</param>
    /// <param name="objs">�˻��� GameObjects</param>
    /// <returns>target�� objs���� ��� ��ü</returns>
    protected List<GameObject> GetExistingTargets(CharacterBase caster, Define.SkillTarget target, GameObject[] objs)
    {
        var targets = GetTargets(caster, target);
        var existingTargets = new List<GameObject>();

        foreach (var obj in objs)
        {
            // CharacterManager�� ��ϵ��� ���� obj�� ��ŵ
            // Ground�� ��쵵 �޾ƿ� Ground ����Ʈ �ȿ� �������� �ʴ� ��� �Ǻ��� ����
            if (!targets.Contains(obj)) { continue; }

            // ��ϵ� obj�� �߰�
            existingTargets.Add(obj);
        }

        return existingTargets;
    }

    /// <summary>
    /// Define.SkillTarget�� LayerMask�� ��ȯ�Ͽ� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="target">��ų ���</param>
    /// <returns>����� LayerMask. ��ȯ �Ұ��� ��� -1 ��ȯ</returns>
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