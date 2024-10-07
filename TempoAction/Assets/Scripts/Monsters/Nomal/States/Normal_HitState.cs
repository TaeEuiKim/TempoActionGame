using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_HitState : Normal_State
{
    public Normal_HitState(NormalMonster monster) : base(monster) { }

    public override void Enter()
    {
        base.Enter();

        // ���� ���� ���°� ��ų�����̾��ٸ�
        if (_monster.PreviousPerceptionState == Define.PerceptionType.SKILLATTACK)
        {
            // Skill�� OnEnded�� ȣ��Ǳ���� ���
            return;
        }

        // �ִ� ����

        // ���� ����
        if(_monster.Stat.Hp > 0)
        {
            _monster.CurrentPerceptionState = Define.PerceptionType.IDLE;
        }
        else
        {
            //_monster.CurrentPerceptionState = Define.PerceptionType.DEATH;
        }
    }

    public override void Stay() { }
}
