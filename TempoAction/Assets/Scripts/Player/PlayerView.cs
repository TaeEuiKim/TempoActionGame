using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Image _hpBarImage;
    [SerializeField] private GameObject _gameoverUI;
    [SerializeField] private GameObject[] MainSkillSlots;
    [SerializeField] private GameObject[] SubSkillSlots;
    [SerializeField] private Sprite[] _skillSprite;
    [SerializeField] private Sprite _skillBackIcon;

    private Image[] _mainSkillIcons;
    private Image[] _subSkillIcons;

    private void Awake()
    {
        ResetSlot();
    }

    private void ResetSlot()
    {
        _mainSkillIcons = new Image[MainSkillSlots.Length];
        _subSkillIcons = new Image[SubSkillSlots.Length];
        for (int i = 0; i < MainSkillSlots.Length; i++)
        {
            _mainSkillIcons[i] = MainSkillSlots[i].GetComponent<Image>();
        }
        for (int i = 0; i < SubSkillSlots.Length; i++)
        {
            _subSkillIcons[i] = SubSkillSlots[i].GetComponent<Image>();
        }
    }

    public void ChangeMainSkillIcon(int value, bool isRemove)
    {
        if (isRemove)
        {
            _mainSkillIcons[value].sprite = _skillBackIcon;
        }
        else
        {
            _mainSkillIcons[value].sprite = _skillSprite[value];
        }
    }

    public void ChangeSubSkillIcon(int value, bool isRemove)
    {
        if (isRemove)
        {
            _subSkillIcons[value].sprite = _skillBackIcon;
        }
        else
        {
            _subSkillIcons[value].sprite = _skillSprite[value];
        }
    }

    public void SetSkillIcon(Image[] mainIcon, Image[] subIcon)
    {
        //ResetSlot();
        for (int i = 0; i < mainIcon.Length; ++i)
        {
            _mainSkillIcons[i].sprite = mainIcon[i].sprite;
        }

        for (int i = 0; i < subIcon.Length; ++i)
        {
            _subSkillIcons[i].sprite = subIcon[i].sprite;
        }
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
        _gameoverUI.SetActive(true);
    }

    public void UpdateHpBar(float value)
    {
        _hpBarImage.fillAmount = value;
    }
}
