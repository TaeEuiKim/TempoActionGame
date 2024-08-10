using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(CreatePlatform))]
public class CreatePlatformEditor : Editor
{
    public VisualTreeAsset TreeAsset;
    private CreatePlatform _createPlatform;

    [MenuItem("Tools/Create Platform")]
    public static void Create()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Buff/Platform/TempPlatform.prefab");
        Instantiate(prefab);
    }

    public override VisualElement CreateInspectorGUI()
    {
        if (!TreeAsset)
            return base.CreateInspectorGUI();

        _createPlatform = (CreatePlatform)target;

        VisualElement root = new VisualElement();
        TreeAsset.CloneTree(root);

        // Add your UI content here
        var inputMScript = root.Q<ObjectField>("unity-input-m_Script");
        inputMScript.AddToClassList("unity-disabled");
        inputMScript.Q(null, "unity-object-field__selector")?.SetEnabled(false);
        // root.Q<Label>("title").text = "Custom Property Drawer";

        var createBtn = root.Q<Button>("Button_Create");
        var deleteBtn = root.Q<Button>("Button_Delete");

        createBtn.clickable.clicked += () =>
        {
            _createPlatform.Create();
        };
        deleteBtn.clickable.clicked += () =>
        {
            _createPlatform.Delete();
        };


        return root;
    }
}