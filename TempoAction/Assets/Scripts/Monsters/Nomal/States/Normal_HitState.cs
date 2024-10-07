using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_HitState : Normal_State
{
    public Normal_HitState(NormalMonster monster) : base(monster) { }

    public override void Enter()
    {
        base.Enter();

        // 만약 이전 상태가 스킬공격이었다면
        if (_monster.PreviousPerceptionState == Define.PerceptionType.SKILLATTACK)
        {
            // Skill의 OnEnded가 호출되기까지 대기
            return;
        }

        // 애니 적용

        // 상태 전이
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
