using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillCommand", menuName = "ScriptableObjects/Skill/SkillCommand/SkillCommand", order = 1)]
public class SkillCommand : ScriptableObject
{
    [SerializeField] public CommandData[] commandDatas;

    public float GetDamage(int skillCount)
    {
        return commandDatas.FirstOrDefault(data => data.SkillId == skillCount)?.damage ?? 0;
    }

    public float GetStemina(int skillCount)
    {
        return commandDatas.FirstOrDefault(data => data.SkillId == skillCount)?.useStemina ?? 0;
    }
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
    [Header("������")]
    public float damage;
    [Header("���׹̳� �Ҹ�")]
    public float useStemina;
}
