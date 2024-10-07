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

    public virtual void Run(CharacterBase character, UnityAction OnEnded = null)
    {
        Initialize();
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

            // 이펙트가 살아 있는지 체크
            removingEffects.Clear();
            foreach (var effect in effects)
            {
                if(!effect.IsAlive(true)) { removingEffects.Add(effect); }
            }

            // 죽은 이펙트는 비활성화 및 더이상 검사하지 않도록 삭제
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
            // 무한반복이 아니고 자식 중 하나라도 살아 있다면
            if(!effect.main.loop && effect.IsAlive(true))
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
}