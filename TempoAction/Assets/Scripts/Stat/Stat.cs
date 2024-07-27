using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] protected float _speed;
    public float Speed { get => _speed; set => _speed = value; }

    [SerializeField] protected float _maxHp;
    public float MaxHp { get => _maxHp; set => _maxHp = value; }

    protected float _hp;
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
        }
    }

    [SerializeField] protected float _damage;
    public float Damage { get => _damage; set => _damage = value; }

    protected bool _isDead = false;
   
    
  
    
}
