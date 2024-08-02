using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtkTempoData", menuName = "ScriptableObjects/AtkTempo Data", order = int.MaxValue)]
public class AtkTempoData : ScriptableObject
{
    public Define.TempoType type;
    public float damage;
    public float minStamina;
    public float maxStamina;
    public float distance;
}
