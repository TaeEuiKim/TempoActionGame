using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Normal_IdleState : Normal_State
{
    public Normal_IdleState(NormalMonster monster) : base(monster) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Stay()
    {
        if (_monster.monsterType == Define.NormalMonsterType.BALDO)
        {
            _monster.Direction = -(_monster.Player.position.x - _monster.transform.position.x);
        }
        else if (_monster.monsterType == Define.NormalMonsterType.KUNG || _monster.monsterType == Define.NormalMonsterType.MON3)
        {
            _monster.Direction = _monster.Player.position.x - _monster.transform.position.x;
        }

        if (!_monster.IsTrace && !_monster.isHit)
        {
            float distance = Vector3.Distance(_monster.transform.position, _monster.Target.position);

            // 공격 시도(스킬 or 일반)
            if (!_monster.TryAttack())
            {
                // 공격 범위 밖, 인지 범위 내(추격)
                if (distance > _monster.MonsterSt.AttackRange && distance <= _monster.PerceptionDistance * SkillData.cm2m)
                {
                    _monster.CurrentPerceptionState = Define.PerceptionType.TRACE;
                }
            }
        }
        else
        {
            _monster.CurrentPerceptionState = Define.PerceptionType.TRACE;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
