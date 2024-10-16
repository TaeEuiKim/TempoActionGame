using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Dropbomb", menuName = "ScriptableObjects/MiddleMonster/Skill/Dropbomb", order = 1)]
public class Middle_Dropbomb : Middle_Skill
{
    [Header("��ź ����")]
    [SerializeField] private int bombAmount;  
    [Header("�ʱ� �̻��� �ӵ�")]
    [SerializeField] private float initSpeed = 10f; 
    [Header("���� ����")]
    [SerializeField] private float strength = 5f;
    [Header("�ִ� �̻��� �ӵ�")]
    [SerializeField] private float missleSpeed = 10f;

    private int _bombCount = 0;

    private float _coolTime = 0f;

    public override void Init(MiddleMonster monster)
    {
        base.Init(monster);

        _bombCount = 0;
        _coolTime = 0;
    }

    public override void Check()
    {
        if (IsCompleted) return;

        if (_coolTime >= _info.coolTime) // ��Ÿ�� Ȯ��
        {
            IsCompleted = true;
        }
        else
        {
            _coolTime += Time.deltaTime;
        }
    }

    public override void Enter()
    {
        Debug.Log("���� �̻��� �߻�");

        _coolTime = 0;

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
}
    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Dropbomb"))
        {
            _monster.Ani.SetBool("Dropbomb", true);
        }
    }

    public override void Exit()
    {
        if (_monster.Ani.GetBool("Dropbomb"))
        {
            _monster.Ani.SetBool("Dropbomb", false);
        }

        _coolTime = 0;
    }

    private void Attack()
    {
        GameObject bomb = ObjectPool.Instance.Spawn("TraceRocket");
        bomb.transform.position = _monster.HitPoint.position + new Vector3(0, 0, -1f);

        bomb.GetComponent<Dropbomb>().SettingValue(_monster.Player, Info.damage, initSpeed, strength, missleSpeed);
    }

    private void Finish()
    {
        IsCompleted = false;
        _monster.FinishSkill();
    }
}
