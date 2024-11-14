using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillCommand", menuName = "ScriptableObjects/Skill/SkillCommand/SkillCommand", order = 1)]
public class SkillCommand : ScriptableObject
{
    [SerializeField] public CommandData[] commandDatas;
}

[Serializable]
public class CommandData
{
    [Header("��ų �̸�")]
    public string name;
    [Header("��ų ���̵�")]
    public int SkillId;
    [Header("���� ��ų ���̵�")]
    public int[] PossibleSkillId;
    [Header("�� �뽬 ���� ����")]
    public bool IsBack;
    [Header("�� �뽬 ���� ����")]
    public bool IsFront;
    [Header("��ų Ŀ�ǵ�")]
    public KeyCode[] KeyCodes;
}
