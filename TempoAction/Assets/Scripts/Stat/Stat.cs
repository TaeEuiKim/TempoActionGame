using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{

    [SerializeField] protected float _maxHealthPoints;
    [SerializeField] protected float _healthPoints;

    [SerializeField] protected float _walkSpeed;
    [SerializeField] protected float _sprintSpeed;

    [SerializeField] protected float _attackDamage;

    protected bool _isDead = false;


    public float MaxHealthPoints { get => _maxHealthPoints; set => _maxHealthPoints = value; }
    public virtual float HealthPoints
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
        }
    }
    public float WalkSpeed { get => _walkSpeed; set => _walkSpeed = value; } 
    public float SprintSpeed { get => _sprintSpeed; set => _sprintSpeed = value; }
    public float AttackDamage { get => _attackDamage; set => _attackDamage = value; }

}
