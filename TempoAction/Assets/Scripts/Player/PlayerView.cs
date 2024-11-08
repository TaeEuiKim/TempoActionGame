using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Image _hpBarImage;
    [SerializeField] private Image _steminaBarImage;
    [SerializeField] private Image _ultimateBarImage;
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

    public void UpdateUltimateGauge(float value)
    {
        if (!_ultimateBarImage) { return; }

        StartCoroutine(ChangeUltimateGauge(value));
    }

    private IEnumerator ChangeUltimateGauge(float value)
    {
        while (_hpBarImage.fillAmount > value)
        {
            _ultimateBarImage.fillAmount -= Time.deltaTime;

            yield return null;
        }

        if (_ultimateBarImage.fillAmount < value)
        {
            _ultimateBarImage.fillAmount = value;
        }

        yield return null;
    }

    public void UpdateHpBar(float value)
    {
        if(!_hpBarImage) { return; }

        StartCoroutine(UpdateHealthBar(value));
    }

    private IEnumerator UpdateHealthBar(float value)
    {
        while (_hpBarImage.fillAmount > value)
        {
            _hpBarImage.fillAmount -= Time.deltaTime;

            yield return null;
        }

        if (_hpBarImage.fillAmount < value)
        {
            _hpBarImage.fillAmount = value;
        }

        yield return null;
    }
}
