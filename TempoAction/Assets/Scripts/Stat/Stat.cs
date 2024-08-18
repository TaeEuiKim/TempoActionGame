using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{

    [SerializeField] protected float _maxHp;
    public float MaxHp { get => _maxHp; set => _maxHp = value; }

    [SerializeField] protected float _hp;
    public virtual float Hp
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

    [SerializeField] protected float _walkSpeed;
    public float WalkSpeed { get => _walkSpeed; set => _walkSpeed = value; }

    [SerializeField] protected float _sprintSpeed;
    public float SprintSpeed { get => _sprintSpeed; set => _sprintSpeed = value; }


  

    [SerializeField] protected float _attackDamage;
    public float AttackDamage { get => _attackDamage; set => _attackDamage = value; }

    protected bool _isDead = false;
   
    
  
    
}
