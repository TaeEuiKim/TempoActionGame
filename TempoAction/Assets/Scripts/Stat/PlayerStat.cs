using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : Stat
{

    public override float Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                _hp = 0;
                _isDead = true;
            }
            else if(_hp > _maxHp)
            {
                _hp = _maxHp;
            }
            ShowHp();
        }
    }

    [SerializeField] private float _jumpForce;// 점프 힘
    public float JumpForce { get => _jumpForce; set => _jumpForce = value; }
   
    private float _stamina;
    public float Stamina
    {
        get
        {
            return _stamina;
        }
        set
        {
            _stamina = value;

            if (_stamina >= _maxStamina)
            {
                _stamina = _maxStamina;
            }
            else if (_stamina < 0)
            {
                _stamina = 0;
            }
        

            UIManager.Instance.GetUI<Image>("StaminaImage").fillAmount = _stamina / _maxStamina;
        }
    }


    [SerializeField] private float _dashDistance = 5f;
    public float DashDistance { get => _dashDistance; }

    [SerializeField] private float _dashDuration = 0.2f;
    public float DashDuration { get => _dashDuration; }


    [SerializeField] private float _maxStamina;
    public float MaxStamina { get => _maxStamina; }


    [SerializeField] private float _stunDelay; // 과부화 시 스턴까지 걸리는 시간
    public float StunDelay { get => _stunDelay; }// 스턴 상태 시간


    [SerializeField] private float _stunTime; // 스턴 상태 시간
    public float StunTime { get => _stunTime; }// 스턴 상태 시간


    private void Start()
    {
        _hp = _maxHp;
    }

    public bool CheckOverload()
    {
        if (_stamina == _maxStamina)
        {
            return true;
        }
        return false;
    }

    public void ShowHp()
    {
        UIManager.Instance.GetUI<Image>("Hpbar").fillAmount = _hp / MaxHp;
    }
}
