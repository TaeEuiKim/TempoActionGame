using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Normal_TraceState : Normal_State
{
    float timer = 0f;

    public Normal_TraceState(NormalMonster monster) : base(monster)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(_monster.Ani == null) { return; }

        if (!_monster.Ani.GetBool("Run"))
        {
            _monster.Ani.SetBool("Run", true);
        }
    }

    public override void Stay()
    {
        if (timer < 1.5f)
        {
            timer += Time.deltaTime;
            if (_monster.Stat.Hp <= 0)
            {
                _monster.CurrentPerceptionState = Define.PerceptionType.DEATH;
            }
        }

        if (!_monster.IsTrace)
        {
            float dir = _monster.Target.transform.position.x - _monster.transform.position.x;
            var tempVelocity = new Vector2(dir * _monster.Stat.WalkSpeed * Time.deltaTime, _monster.Rb.velocity.y);

            _monster.Rb.velocity = tempVelocity;

            if (_monster.monsterType == Define.NormalMonsterType.BALDO)
            {
                _monster.Direction = -dir;
            }
            else if (_monster.monsterType == Define.NormalMonsterType.KUNG || _monster.monsterType == Define.NormalMonsterType.MON3)
            {
                _monster.Direction = dir;
            }

            if (_monster.TryAttack()) 
            {
                return; 
            }

            float distance = Vector3.Distance(_monster.transform.position, _monster.Target.position);
            if (distance <= _monster.MonsterSt.AttackRange || distance > _monster.PerceptionDistance * SkillData.cm2m)
            {
                _monster.CurrentPerceptionState = Define.PerceptionType.IDLE;
            }
        }
        else
        {
            float dir = _monster.Target.transform.position.x - _monster.transform.position.x;
            var tempVelocity = new Vector2(dir * _monster.Stat.WalkSpeed * Time.deltaTime, _monster.Rb.velocity.y);

            _monster.Rb.velocity = tempVelocity;

            if (_monster.monsterType == Define.NormalMonsterType.BALDO)
            {
                _monster.Direction = -dir;
            }
            else if (_monster.monsterType == Define.NormalMonsterType.KUNG || _monster.monsterType == Define.NormalMonsterType.MON3)
            {
                _monster.Direction = dir;
            }

            if (_monster.TryAttack())
            {
                return;
            }
        }

        //_monster.CurrentPerceptionState = Define.PerceptionType.GUARD;
    }

    public override void Exit()
    {
        _monster.Ani?.SetBool("Run", false);

        base.Exit();
        timer = 0;
    }
}
