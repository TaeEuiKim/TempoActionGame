using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEffect : MonoBehaviour
{
    private PlayerManager _player;

    [SerializeField] private GameObject _effectR;
    [SerializeField] private GameObject _effectL;

    [SerializeField] private bool _isOn = false;
    private bool _last = false;

    private void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        if (_isOn == _last) return;

        if (_isOn)
        {
            OnStunEffect();
        }
        else
        {
            OffStunEffect();
        }

        _last = _isOn;
    }


    private void OnStunEffect()
    {
        if (_player.Controller.Direction > 0)
        {
            _effectR.SetActive(true);
        }
        else
        {
            _effectL.SetActive(true);
        }
    }

    private void OffStunEffect()
    {
        if (_player.Controller.Direction > 0)
        {
            _effectR.SetActive(false);
        }
        else
        {
            _effectL.SetActive(false);
        }
    }
}
