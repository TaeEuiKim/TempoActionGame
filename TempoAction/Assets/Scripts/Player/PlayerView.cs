using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Image _hpBarImage;
    [SerializeField] private GameObject _gameoverUI;

    public void OnGameoverUI()
    {
        _gameoverUI.SetActive(true);
    }

    public void UpdateHpBar(float value)
    {
        _hpBarImage.fillAmount = value;
    }
}
