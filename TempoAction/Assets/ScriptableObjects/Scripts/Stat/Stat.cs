using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : ScriptableObject
{

    [SerializeField] protected float _maxHealth;
    protected float _health;
    [SerializeField] protected float _defense;

    [SerializeField] protected float _walkSpeed;
    [SerializeField] protected float _sprintSpeed;

    [SerializeField] protected float _damage;

    protected bool _isDead = false;


    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float Defense { get => _defense; set => _defense = value; }
    public float WalkSpeed { get => _walkSpeed; set => _walkSpeed = value; } 
    public float SprintSpeed { get => _sprintSpeed; set => _sprintSpeed = value; }
    public float Damage { get => _damage; set => _damage = value; }


    public abstract void Initialize();
}
