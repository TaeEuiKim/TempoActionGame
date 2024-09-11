using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Image _hpBarImage;

    public void UpdateHpBar(float value)
    {
        _hpBarImage.fillAmount = value;
    }
}
