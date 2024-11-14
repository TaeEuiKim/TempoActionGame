using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [SerializeField] protected Stat _stat;
    protected Animator _ani;
    protected Rigidbody _rb;
    protected ISkillManager _skillManager;
    [SerializeField] protected Transform _characterModel;
    [SerializeField] protected CharacterColliderManager _colliderManager;
    [SerializeField] protected Vector3 _rayOffset = Vector3.zero;

    public Stat Stat { get => _stat; set => _stat = value; }
    public Rigidbody Rb { get { return _rb; } }
    public Animator Ani { get { return _ani; } }
    public Transform CharacterModel { get { return _characterModel; } }
    public ISkillManager SkillManager { get { return _skillManager; } }
    public CharacterColliderManager ColliderManager { get { return _colliderManager; } }

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _ani = GetComponentInChildren<Animator>();
        _skillManager = GetComponent<ISkillManager>();
        _colliderManager.Initialize();

        _stat.Init();
    }

    protected virtual void Update()
    {
        _skillManager?.OnUpdate(this);
    }

    public virtual bool IsLeftDirection()
    {
        return _characterModel.localScale.x > 0;
    }

    /// <summary>
    /// 이 캐릭터에게서 쏠 Ray의 출발 지점을 가져옵니다.
    /// </summary>
    /// <returns>Origin Position</returns>
    public virtual Vector3 GetRayOrigin()
    {
        return transform.position + _rayOffset;
    }

    public abstract void TakeDamage(float damage); 
}
