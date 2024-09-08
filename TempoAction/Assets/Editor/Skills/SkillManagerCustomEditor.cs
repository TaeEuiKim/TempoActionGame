using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillManager))]
public class SkillManagerCustomEditor : Editor
{
    private SerializedObject serializedObjectTarget;
    private SerializedProperty skillSlotCount;
    private SerializedProperty reserveSlotCount;
    private SerializedProperty skillSlots;
    public void OnEnable()
    {
        serializedObjectTarget = new SerializedObject(target);
        skillSlotCount = serializedObjectTarget.FindProperty("MaxSkillSlot");
        reserveSlotCount = serializedObjectTarget.FindProperty("MaxReserveSlot");
        skillSlots = serializedObjectTarget.FindProperty("skillSlots");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SkillManager manager = (SkillManager)target;

        manager.Initialize();
    }
}
