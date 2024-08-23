using UnityEngine;

[CreateAssetMenu(fileName = "TempoAttackData", menuName = "ScriptableObjects/TempoAttack Data", order = int.MaxValue)]
public class TempoAttackData : ScriptableObject
{
    public Define.TempoType type;

    public float minDamage;
    public float maxDamage;
    public float distance;

    public float minStamina;
    public float maxStamina;
}