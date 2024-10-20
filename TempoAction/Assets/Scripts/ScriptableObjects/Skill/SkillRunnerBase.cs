using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

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
    public abstract void Initialize();
    public abstract IEnumerator SkillCoroutine(CharacterBase character);
    private IEnumerator WaitForSkillEnded(CharacterBase character, UnityAction OnFinished)
    {
        while(true)
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
                if(!effect.IsAlive(true)) { removingEffects.Add(effect); }
            }

            // ���� ����Ʈ�� ��Ȱ��ȭ �� ���̻� �˻����� �ʵ��� ����
            foreach (var effect in removingEffects)
            {
                effect.gameObject.SetActive(false);
                effects.Remove(effect);
            }
        }
    }
    protected void ActiveEffectToCharacter(CharacterBase character, GameObject effect)
    {
        effect.transform.position = character.transform.position;
        effect.SetActive(true);
    }

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
}