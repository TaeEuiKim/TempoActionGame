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
    }

    public override void Stay()
    {
        if (timer < 1.5f)
        {
            timer += Time.deltaTime;
            if (_monster.Stat.Hp <= 0)
            {
                _monster.CurrentPerceptionState = Define.PerceptionType.DEATH;
                Debug.Log(_monster.name + "ÀÇ Ã¼·Â : " +  _monster.Stat.Hp);
            }
        }

        float dir = _monster.Target.transform.position.x - _monster.transform.position.x;
        var tempVelocity = new Vector2(dir * _monster.Stat.WalkSpeed * Time.deltaTime, _monster.Rb.velocity.y);

        _monster.Rb.velocity = tempVelocity;
        _monster.Direction = -dir;

        if (_monster.TrySkillAttack()) { return; }

        float distance = Vector3.Distance(_monster.transform.position, _monster.Target.position);
        if (distance > _monster.PerceptionDistance * SkillData.cm2m)
        {
            _monster.CurrentPerceptionState = Define.PerceptionType.IDLE;
        }
        //_monster.CurrentPerceptionState = Define.PerceptionType.GUARD;
    }

    public override void Exit()
    {
        base.Exit();
        timer = 0;
    }
}
