using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    [Header("파괴 가능 여부")]
    [SerializeField] protected bool isDestory;
    [Header("최대 체력")]
    [SerializeField] protected float maxHp;
    [Header("현재 체력")]
    private float _curHp;

    [Space]

    [Header("애니메이션")]
    private Animator _animator;

    public Animator Ani {  get { return _animator; } }
    public float Hp { get { return _curHp; } }

    protected virtual void Awake()
    {
        TryGetComponent<Animator>(out _animator);

        _curHp = maxHp;
    }

    public virtual void TakeDamage(float value)
    {
        if (!isDestory)
        {
            return;
        }

        _curHp -= value;
    }
}
