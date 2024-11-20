using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class MonsterView : MonoBehaviour
{
    [SerializeField] protected List<Image> _hpBarImage;
    [SerializeField] protected List<Image> _hpIllusionBarImage;

    private float time = 0.02f;
    private WaitForSeconds seconds;

    private int count = 1;

    private void Start()
    {
        seconds = new WaitForSeconds(time);
    }

    public void UpdateHpBar(float value)
    {
        StartCoroutine(UpdateHealthBar(value));
    }

    public int GetDevideHp()
    {
        return _hpBarImage.Count;
    }

    private IEnumerator UpdateHealthBar(float value)
    {
        float fillAmount = value;

        if (fillAmount < 0 || value == 0)
        {
            fillAmount = 0;
        }

        StartCoroutine(UpdateIllusionBar(fillAmount));

        while (_hpBarImage[_hpBarImage.Count - count].fillAmount >= fillAmount)
        {
            _hpBarImage[_hpBarImage.Count - count].fillAmount -= time;

            yield return time;
        }

        _hpBarImage[_hpBarImage.Count - count].fillAmount = fillAmount;

        yield return null;
    }

    private IEnumerator UpdateIllusionBar(float value)
    {
        yield return new WaitForSeconds(0.05f);

        while (_hpIllusionBarImage[_hpIllusionBarImage.Count - count].fillAmount >= value)
        {
            _hpIllusionBarImage[_hpIllusionBarImage.Count - count].fillAmount -= time;

            yield return time;
        }

        _hpIllusionBarImage[_hpIllusionBarImage.Count - count].fillAmount = value;

        yield return null;
    }
}
