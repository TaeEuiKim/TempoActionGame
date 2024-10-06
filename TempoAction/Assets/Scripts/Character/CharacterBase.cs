using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    protected Animator _ani;
    protected Rigidbody _rb;
    protected ISkillManager _skillManager;
    [SerializeField] protected Transform _characterModel;

    public Rigidbody Rb { get { return _rb; } }
    public Animator Ani { get { return _ani; } }
    public Transform CharacterModel { get { return _characterModel; } }
    public ISkillManager SkillManager { get { return _skillManager; } }

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
