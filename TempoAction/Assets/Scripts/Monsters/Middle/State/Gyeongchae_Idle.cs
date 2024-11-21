using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Gyeongchae_Idle : Middle_State
{
    private float timer = 0f;
    private float idleTime;

    public Gyeongchae_Idle(MiddleMonster monster) : base(monster)
    {

    }

    public override void Enter()
    {
        idleTime = 0;
    }

    public override void Stay()
    {
        if (timer < 3f)
        {
            timer += Time.deltaTime;
            return;
        }

        if (idleTime < _monster.IdleDuration)
        {
            Stop();
            idleTime += Time.deltaTime;
        }
        else
        {
            foreach (Middle_Skill s in _monster.SkillStorage)
            {
                if (s.Info.level == _monster.phase)
                {
                    s.Check();

                    if (s.IsCompleted) // 조건이 성립되었는지 확인
                    {
                        _monster.ReadySkills.Add(s);
                    }
                }
            }

            if (_monster.ReadySkills.Count <= 0) return;

            Middle_Skill prioritySkill = _monster.ReadySkills[0];

            if (_monster.ReadySkills.Count > 1) // 2개 이상일 때 우선순위 확인
            {
                for (int i = 1; i < _monster.ReadySkills.Count; i++)
                {
                    if (prioritySkill.Info.priority < _monster.ReadySkills[i].Info.priority)
                    {
                        prioritySkill = _monster.ReadySkills[i];
                    }
                }
            }

            _monster.ChangeCurrentState(Define.MiddleMonsterState.USESKILL);
            _monster.SkillStorage.Remove(prioritySkill);
            _monster.ChangeCurrentSkill(prioritySkill);
            _monster.ReadySkills.Clear();
        }
    }

    public override void Exit()
    {
        Stop();
    }

    private void Stop()
    {
        _monster.Rb.velocity = new Vector2(0, _monster.Rb.velocity.y);
    }
}
