using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
   
 


    [Header("대쉬")]
    [SerializeField] private float _dashDelay = 5f;
    public float DashDelay { get => _dashDelay; }

    [SerializeField] private float _dashDistance = 5f;
    public float DashDistance { get => _dashDistance; }

    [SerializeField] private float _dashDuration = 0.2f;
    public float DashDuration { get => _dashDuration; }

    [Header("스태미나")]
    [SerializeField] private float _maxStamina;
    public float MaxStamina { get => _maxStamina; }

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

    [Header("스턴")]
    [SerializeField] private float _stunDelay; // 과부화 시 스턴까지 걸리는 시간
    public float StunDelay { get => _stunDelay; }// 스턴 상태 시간


    [SerializeField] private float _stunTime; // 스턴 상태 시간
    public float StunTime { get => _stunTime; }// 스턴 상태 시간


    private bool _isKnockedBack = false;
    public bool IsKnockedBack { get => _isKnockedBack; set => _isKnockedBack = value; }

    private void Start()
    {
        _hp = _maxHp;
    }

    public void TakeDamage(float damage)
    {
        if (_isKnockedBack) return;

        Hp -= damage;

        
    }

    public void Knockback(Vector2 knockbackDirection, float t = 0)
    {
        StartCoroutine(StartKnockBack(knockbackDirection, t));
    }

    public IEnumerator StartKnockBack(Vector2 knockbackDirection, float t)
    {
        _isKnockedBack = true;
        GetComponent<Rigidbody>().AddForce(knockbackDirection, ForceMode.Impulse);

        yield return new WaitForSeconds(t);

        GetComponent<Rigidbody>().velocity = Vector2.zero;
        _isKnockedBack = false;
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
