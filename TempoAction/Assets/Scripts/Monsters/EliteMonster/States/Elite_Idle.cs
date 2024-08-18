using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite_Idle : Elite_State
{
    private float idleTime;

    public Elite_Idle(EliteMonster monster) : base(monster)
    {

    }

    public override void Enter()
    {
        idleTime = 0;
    }
    public override void Stay()
    {
        Follow();

        if (idleTime < _monster.IdleDuration)
        {
            idleTime += Time.deltaTime;
        }
        else
        {
            foreach (Elite_Skill s in _monster.Phase1SkillStorage)
            {
                s.Check(); // 조건 확인

                if (s.IsCompleted) // 조건이 성립되었는지 확인
                {
                    _monster.ReadySkills.Add(s);
                }
            }

            if (_monster.ReadySkills.Count <= 0) return;

            Elite_Skill prioritySkill = _monster.ReadySkills[0];
            _monster.Phase1SkillStorage.Remove(prioritySkill);

            if (_monster.ReadySkills.Count > 1) // 2개 이상일 때 우선순위 확인
            {
                for (int i = 1; i < _monster.ReadySkills.Count; i++)
                {
                    if (prioritySkill.Info.priority < _monster.ReadySkills[i].Info.priority)
                    {
                        prioritySkill = _monster.ReadySkills[i];
                    }

                    _monster.Phase1SkillStorage.Remove(_monster.ReadySkills[i]);
                }
            }
            _monster.CurrentState = Define.EliteMonsterState.USESKILL;
            _monster.CurrentSkill = prioritySkill;
            _monster.ReadySkills.Remove(prioritySkill);

        }

    }
    public override void Exit()
    {

    }

    private void Follow()
    {
        float direction = _monster.Player.transform.position.x - _monster.transform.position.x;
        _monster.Direction = direction;

        if (Mathf.Abs(direction) <= _monster.Stat.AttackRange)
        {

        }
        else
        {
            _monster.Rb.velocity = new Vector2(_monster.Direction * _monster.Stat.SprintSpeed, _monster.Rb.velocity.y);
        }
    }
}
