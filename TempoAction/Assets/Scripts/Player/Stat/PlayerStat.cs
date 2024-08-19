using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerStat : Stat
{

    [SerializeField] private float _jumpForce;// 점프 힘

    [Header("대쉬")]
    [SerializeField] private float _dashDelay = 5f;
    [SerializeField] private float _dashDistance = 5f;
    [SerializeField] private float _dashDuration = 0.2f;

    [Header("스태미나")]
    [SerializeField] private float _maxStamina;
    private float _stamina;

    [Header("스턴")]
    [SerializeField] private float _stunDelay; // 과부화 시 스턴까지 걸리는 시간
    [SerializeField] private float _stunTime; // 스턴 상태 시간
 
    private bool _isKnockedBack = false;


    public override float HealthPoints
    {
        get
        {
            return _healthPoints;
        }
        set
        {
            _healthPoints = value;
            if (_healthPoints <= 0)
            {
                _healthPoints = 0;
                _isDead = true;
            }
            else if (_healthPoints > _maxHealthPoints)
            {
                _healthPoints = _maxHealthPoints;
            }
            ShowHp();
        }
    }

    public float JumpForce { get => _jumpForce; set => _jumpForce = value; }
    public float DashDelay { get => _dashDelay; }
    public float DashDistance { get => _dashDistance; }
    public float DashDuration { get => _dashDuration; }
    public float MaxStamina { get => _maxStamina; }
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
    public float StunDelay { get => _stunDelay; }// 스턴 상태 시간
    public float StunTime { get => _stunTime; }// 스턴 상태 시간
    public bool IsKnockedBack { get => _isKnockedBack; set => _isKnockedBack = value; }


    private void Start()
    {
        _healthPoints = _maxHealthPoints;
    }

    public void TakeDamage(float damage)
    {
        if (_isKnockedBack) return;

        HealthPoints -= damage;
    }

    //넉백 함수
    public void Knockback(Vector2 knockbackDirection, float t = 0)
    {
        StartCoroutine(StartKnockBack(knockbackDirection, t));
    }
    // 넉백 시작
    public IEnumerator StartKnockBack(Vector2 knockbackDirection, float t)
    {
        _isKnockedBack = true;
        GetComponent<Rigidbody>().AddForce(knockbackDirection, ForceMode.Impulse);

        yield return new WaitForSeconds(t);

        GetComponent<Rigidbody>().velocity = Vector2.zero;
        _isKnockedBack = false;
    }

    // 과부화 상태인지 확인(스테미너가 최대 스테미나랑 같을 때)
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
        UIManager.Instance.GetUI<Image>("Hpbar").fillAmount = _healthPoints / _maxHealthPoints;
    }
}
