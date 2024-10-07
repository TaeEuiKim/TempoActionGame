using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillManagerCustomEditor<T> : Editor where T  : MonoBehaviour, ISkillManager
{
/*    private SerializedObject serializedObjectTarget;
    private SerializedProperty skillSlotCount;
    private SerializedProperty reserveSlotCount;
    private SerializedProperty skillSlots;
    public void OnEnable()
    {
        serializedObjectTarget = new SerializedObject(target);
        skillSlotCount = serializedObjectTarget.FindProperty("MaxSkillSlot");
        reserveSlotCount = serializedObjectTarget.FindProperty("MaxReserveSlot");
        skillSlots = serializedObjectTarget.FindProperty("skillSlots");
    }*/

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        T manager = (T)target;

        manager.Initialize();
    }
}

[CustomEditor(typeof(PlayerSkillManager))]
public class PlayerSkillManagerCustomEditor : SkillManagerCustomEditor<PlayerSkillManager> { }

[CustomEditor(typeof(MonsterSkillManager))]
public class MonsterSkillManagerCustomEditor : SkillManagerCustomEditor<MonsterSkillManager> { }