using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Image _hpBarImage;
    [SerializeField] private Image _hpIllusionBarImage;

    [SerializeField] private Image _steminaBarImage;
    [SerializeField] private Image _steminaIllusionBarImage;

    [SerializeField] private Image _ultimateBarImage;
    [SerializeField] private Image _ultimateIllusionBarImage;

    [SerializeField] private GameObject _gameoverUI;
    [SerializeField] private GameObject[] MainSkillSlots;
    [SerializeField] private GameObject[] SubSkillSlots;
    [SerializeField] private Sprite[] _skillSprite;
    [SerializeField] private Sprite _skillBackIcon;

    private Image[] _mainSkillIcons;
    private Image[] _subSkillIcons;

    private bool isUltimate = false;

    public void ChangeMainSkillIcon(int value, bool isRemove)
    {
        //if(_mainSkillIcons.Length <= value) { return; }
        //if (_mainSkillIcons[value] == null) { return; }

        //if (isRemove)
        //{
        //    _mainSkillIcons[value].sprite = _skillBackIcon;
        //    Debug.Log("메인 제거");
        //}
        //else
        //{
        //    _mainSkillIcons[value].sprite = _skillSprite[value];
        //}
    }

    public void ChangeSubSkillIcon(int value, bool isRemove)
    {
        //if (_subSkillIcons.Length <= value) { return; }
        //if (_subSkillIcons[value] == null) { return; }

        //if (isRemove)
        //{
        //    _subSkillIcons[value].sprite = _skillBackIcon;
        //}
        //else
        //{
        //    _subSkillIcons[value].sprite = _skillSprite[value];
        //}
    }

    public void SetSkillIcon(Image[] mainIcon, Image[] subIcon)
    {
        //ResetSlot();
        //for (int i = 0; i < mainIcon.Length; ++i)
        //{
        //    _mainSkillIcons[i].sprite = mainIcon[i].sprite;
        //}

        //for (int i = 0; i < subIcon.Length; ++i)
        //{
        //    _subSkillIcons[i].sprite = subIcon[i].sprite;
        //}
    }

    public Image[] GetMainIcon()
    {
        return _mainSkillIcons;
    }

    public Image[] GetSubIcon()
    {
        return _subSkillIcons;
    }

    public void OnGameoverUI()
    {
        _gameoverUI?.SetActive(true);
    }

    public void MoveUltimateUI(GameObject obj, float value)
    {
        //obj.transform.DOMove(_ultimateBarImage.transform.position, 1f);
        UpdateUltimateGauge(value);
    }

    public void UpdateUltimateGauge(float value)
    {
        if (!_ultimateBarImage) { return; }

        StartCoroutine(ChangeUltimateGauge(value));
    }

    private IEnumerator ChangeUltimateGauge(float value)
    {
        float time = 0.1f;
        WaitForSeconds seconds = new WaitForSeconds(time);
        float fillAmount = _ultimateIllusionBarImage.fillAmount + value;
        if (fillAmount > 1)
        {
            fillAmount = 1;
        }

        while (_ultimateIllusionBarImage.fillAmount < fillAmount)
        {
            _ultimateIllusionBarImage.fillAmount += time;

            yield return time;
        }

        _ultimateIllusionBarImage.fillAmount = fillAmount;

        while (_ultimateBarImage.fillAmount < fillAmount)
        {
            _ultimateBarImage.fillAmount += time;

            yield return time;
        }

        _ultimateBarImage.fillAmount = fillAmount;

        yield return null;
    }

    public void UseUltimate()
    {
        StartCoroutine(ReduceUltimate());
    }

    private IEnumerator ReduceUltimate()
    {
        float time = 0.1f;
        WaitForSeconds seconds = new WaitForSeconds(time);

        while (_ultimateBarImage.fillAmount > 0)
        {
            _ultimateBarImage.fillAmount -= time;

            yield return time;
        }

        _ultimateBarImage.fillAmount = 0;

        while (_ultimateIllusionBarImage.fillAmount > 0)
        {
            _ultimateIllusionBarImage.fillAmount -= time;

            yield return time;
        }

        _ultimateIllusionBarImage.fillAmount = 0;
    }

    public float GetUltimateGauge()
    {
        return _ultimateBarImage.fillAmount;
    }

    public void UpdateHpBar(float value)
    {
        if(!_hpBarImage) { return; }

        StartCoroutine(UpdateHealthBar(value));
    }

    private IEnumerator UpdateHealthBar(float value)
    {
        float time = 0.01f;
        WaitForSeconds seconds = new WaitForSeconds(time);

        if (value < 0)
        {
            value = 0;
        }

        if (value > _hpBarImage.fillAmount)
        {
            StartCoroutine(UpdateHPIllusionBar(value));
            while (_hpIllusionBarImage.fillAmount <= value)
            {
                _hpIllusionBarImage.fillAmount += time;

                yield return seconds;
            }

            _hpIllusionBarImage.fillAmount = value;

            yield return null;
        }
        else
        {
            StartCoroutine(UpdateHPIllusionBar(value));
            while (_hpBarImage.fillAmount >= value)
            {
                _hpBarImage.fillAmount -= time;

                yield return seconds;
            }

            _hpBarImage.fillAmount = value;

            yield return null;
        }

        yield return null;
    }

    private IEnumerator UpdateHPIllusionBar(float value)
    {
        float time = 0.01f;
        WaitForSeconds seconds = new WaitForSeconds(time);

        yield return new WaitForSeconds(0.05f);

        if (value < 0)
        {
            value = 0;
        }

        if (value > _hpBarImage.fillAmount)
        {
            while (_hpBarImage.fillAmount <= value)
            {
                _hpBarImage.fillAmount += time;

                yield return seconds;
            }

            _hpBarImage.fillAmount = value;

            yield return null;
        }
        else
        {
            while (_hpIllusionBarImage.fillAmount >= value)
            {
                _hpIllusionBarImage.fillAmount -= time;

                yield return time;
            }

            _hpIllusionBarImage.fillAmount = value;

            yield return null;
        }

        yield return null;
    }

    public void UpdateSteminaBar(float value)
    {
        if (!_steminaBarImage) { return; }

        StartCoroutine(UpdateSteBar(value));
    }

    private IEnumerator UpdateSteBar(float value)
    {
        float time = 0.01f;
        WaitForSeconds seconds = new WaitForSeconds(time);

        if (value < 0)
        {
            value = 0;
        }

        if (value > _steminaBarImage.fillAmount)
        {
            StartCoroutine(UpdateSteIllusionBar(value));

            yield return null;
        }
        else
        {
            StartCoroutine(UpdateSteIllusionBar(value));
            while (_steminaBarImage.fillAmount >= value)
            {
                _steminaBarImage.fillAmount -= time;

                yield return seconds;
            }

            _steminaBarImage.fillAmount = value;

            yield return null;
        }

        yield return null;
    }

    private IEnumerator UpdateSteIllusionBar(float value)
    {
        float time = 0.01f;
        WaitForSeconds seconds = new WaitForSeconds(time);

        yield return new WaitForSeconds(0.05f);

        if (value < 0)
        {
            value = 0;
        }

        if (value > _steminaBarImage.fillAmount)
        {
            while (_steminaBarImage.fillAmount <= value)
            {
                _steminaBarImage.fillAmount += time;

                yield return seconds;
            }

            _steminaBarImage.fillAmount = value;
            _steminaIllusionBarImage.fillAmount = time;

            yield return null;
        }
        else
        {
            while (_steminaIllusionBarImage.fillAmount >= value)
            {
                _steminaIllusionBarImage.fillAmount -= time;

                yield return time;
            }

            _steminaIllusionBarImage.fillAmount = value;

            yield return null;
        }

        yield return null;
    }

    public void AutoUpdateStemina(float value)
    {
        _steminaBarImage.fillAmount = value;
        _steminaIllusionBarImage.fillAmount = value;
    }
}
