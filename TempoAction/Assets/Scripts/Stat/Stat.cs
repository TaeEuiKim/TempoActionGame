using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] protected float _speed;
    public float Speed { get { return _speed; } }

    [SerializeField] protected float _maxHp;
    public float MaxHp { get { return _maxHp; } }

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
    public float Damage { get { return _damage; } }

    protected bool _isDead = false;
   
    
  
    
}
