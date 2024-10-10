using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : ScriptableObject
{

    [SerializeField] protected float _maxHp;
    protected float _hp;
    [SerializeField] protected float _defense;

    [SerializeField] protected float _walkSpeed;
    [SerializeField] protected float _sprintSpeed;

    [SerializeField] protected float _damage;

    protected bool _isDead = false;


    public float MaxHp { get => _maxHp; set => _maxHp = value; }
    public float Defense { get => _defense; set => _defense = value; }
    public float WalkSpeed { get => _walkSpeed; set => _walkSpeed = value; } 
    public float SprintSpeed { get => _sprintSpeed; set => _sprintSpeed = value; }
    public float Damage { get => _damage; set => _damage = value; }

    public float Hp
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
            else if (_hp > _maxHp)
            {
                _hp = _maxHp;
            }
        }
    }

    public abstract void Init();
}
