using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Middle_Idle : Middle_State
{
    private float timer = 0f;

    public Middle_Idle(MiddleMonster monster) : base(monster)
    {

    }

    public override void Enter()
    {
        Debug.Log("idle 진입");
    }

    public override void Stay()
    {
        if (timer < 3f)
        {
            timer += Time.deltaTime;
            return;
        }

        foreach (Middle_Skill s in _monster.SkillStorage)
        {
            s.Check();

            if (s.IsCompleted) // 조건이 성립되었는지 확인
            {
                _monster.ReadySkills.Add(s);
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

    public override void Exit()
    {
    }

    // 플레이어 추적 함수
    private void Follow()
    {
        float direction = _monster.Player.transform.position.x - _monster.transform.position.x;
        _monster.Direction = direction;

        if (Mathf.Abs(direction) <= _monster.Stat.AttackRange)
        {
            _monster.Rb.velocity = new Vector2(0, _monster.Rb.velocity.y);
            //_monster.Ani.SetBool("Run", false);
        }
        else
        {
            _monster.Rb.velocity = new Vector2(_monster.Direction * _monster.Stat.SprintSpeed, _monster.Rb.velocity.y);
            //_monster.Ani.SetBool("Run", true);
        }
    }

    private void Stop()
    {
        //_monster.Ani.SetBool("Run", false);
        _monster.Rb.velocity = new Vector2(0, _monster.Rb.velocity.y);
    }
}
