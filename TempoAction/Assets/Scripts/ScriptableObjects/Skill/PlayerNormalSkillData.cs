using UnityEngine;

[CreateAssetMenu(fileName = "NormalSkillData", menuName = "ScriptableObjects/SkillData/Normal/PlayerNormalSkillData", order = 1)]
public class PlayerNormalSkillData : NormalSkillData, IPlayerSkillData
{
    [field: SerializeField] public int SkillMaxLimit { get;  protected set; }
}