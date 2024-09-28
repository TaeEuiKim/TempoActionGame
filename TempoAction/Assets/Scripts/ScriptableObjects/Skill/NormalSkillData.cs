using UnityEngine;

[CreateAssetMenu(fileName = "NormalSkillData", menuName = "ScriptableObjects/SkillData/NormalSkillData", order = 1)]
public class NormalSkillData : SkillData
{
    // seconds(0.01sec * multiplier)
    public float Cooldown
    {
        get { return cooldown * cooldownMultiplier; }
    }
    // cooldownMultiplier sec (���� ������ 0.01s��� ��)
    [SerializeField] private float cooldown;

    public static float CooldownMultiplier { get { return cooldownMultiplier; } }
    public static float cooldownMultiplier = 0.01f;
}