using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class CharacterBase : MonoBehaviour
{
    [Header("캐릭터 공통")]
    protected Animator _ani;
    protected Rigidbody _rb;
    protected ISkillManager _skillManager;
    [SerializeField] protected Transform _characterModel;
    [SerializeField] protected CharacterColliderManager _colliderManager;

    public Rigidbody Rb { get { return _rb; } }
    public Animator Ani { get { return _ani; } }
    public ISkillManager SkillManager { get { return _skillManager; } }
    public Transform CharacterModel { get { return _characterModel; } }
    public CharacterColliderManager ColliderManager { get { return _colliderManager; } }

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _ani = GetComponentInChildren<Animator>();
        _skillManager = GetComponent<ISkillManager>();
    }

    protected virtual void Update()
    {
        _skillManager?.OnUpdate(this);
    }

    public bool IsLeftDirection()
    {
        return _characterModel.localScale.x < 0;
    }
}
