using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStat", menuName = "ScriptableObjects/Stat/Monster Stat")]
public class MonsterStat : Stat
{

    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _normalAttackCooldown;

    public float AttackRange { get => _attackRange; }
    public float AttackDelay { get => _attackDelay; }
    public float NormalAttackCooldown { get => _normalAttackCooldown; }


    public override void Init()
    {
        _hp = _maxHp;
    }



}
