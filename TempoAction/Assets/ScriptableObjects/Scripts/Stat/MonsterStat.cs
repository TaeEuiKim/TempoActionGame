using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStat", menuName = "ScriptableObjects/Stat/Monster Stat")]
public class MonsterStat : Stat
{

    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackDelay;
        
    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                _health = 0;
                _isDead = true;
            }
            else if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }
        }
    }

    public float AttackRange { get => _attackRange; }
    public float AttackDelay { get => _attackDelay; }


    public override void Initialize()
    {
        _health = _maxHealth;
    }



}
