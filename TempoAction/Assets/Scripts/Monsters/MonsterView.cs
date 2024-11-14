using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MonsterView : MonoBehaviour
{
    [SerializeField] protected Image _hpBarImage;
    [SerializeField] protected Image _hpIllusionBarImage;

    private float time = 0.02f;
    private WaitForSeconds seconds;

    private void Start()
    {
        seconds = new WaitForSeconds(time);
    }

    public void UpdateHpBar(float value)
    {
        StartCoroutine(UpdateHealthBar(value));
    }

    private IEnumerator UpdateHealthBar(float value)
    {
        float fillAmount = value + 0.05f;

        if (fillAmount < 0 || value == 0)
        {
            fillAmount = 0;
        }

        StartCoroutine(UpdateIllusionBar(fillAmount));

        while (_hpBarImage.fillAmount >= fillAmount)
        {
            _hpBarImage.fillAmount -= time;

            yield return time;
        }

        _hpBarImage.fillAmount = fillAmount;

        yield return null;
    }

    private IEnumerator UpdateIllusionBar(float value)
    {
        yield return new WaitForSeconds(0.05f);

        while (_hpIllusionBarImage.fillAmount >= value)
        {
            _hpIllusionBarImage.fillAmount -= time;

            yield return time;
        }

        _hpIllusionBarImage.fillAmount = value;

        yield return null;
    }
}
