using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_GuardState : Normal_State
{
    private MonsterNormalSkillData _skillData;
    private float maxDistance;
    private Vector3 movingPos;
    private Vector3 direction;
    private Vector3 initialPos;

    public Normal_GuardState(NormalMonster monster) : base(monster)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _skillData = (MonsterNormalSkillData)((MonsterSkillSlot)_monster.SkillManager.SkillSlots[0]).skillRunner.skillData;
        maxDistance = _skillData.SkillTriggerValue * SkillData.cm2m;

        /*initialPos = _monster.transform.position;
        movingPos = GetMovingPos();

        direction = (movingPos - _monster.transform.position);
        direction.y = 0;
        direction.Normalize();
        _monster.Direction = direction.x;
        movingPos = CheckWall(movingPos);*/
    }

    public override void Stay()
    {
        movingPos = GetMovingPos();

        _monster.transform.position = Vector3.Lerp(_monster.transform.position, movingPos, 1 - Mathf.Exp(-_monster.Stat.WalkSpeed * Time.deltaTime));
        _monster.Direction = -(_monster.Target.position.x - _monster.transform.position.x);

        /*Vector3 curDir = (movingPos - _monster.transform.position).normalized;
        curDir.y = 0;*/

        /*if (*//*Vector3.Distance(_monster.transform.position, movingPos) > 0.75f && *//*Vector3.Dot(curDir, direction) < 0 )
        {
            *//*movingPos = GetMovingPos();
            direction = (movingPos - _monster.transform.position).normalized;
            direction.y = 0;
            _monster.Direction = direction.x;*//*

            initialPos = _monster.transform.position;

            direction = (movingPos - _monster.transform.position);
            direction.y = 0;
            direction.Normalize();
            _monster.Direction = direction.x;
        }*/

        _monster.TryAttack();
    }

    private Vector3 GetMovingPos()
    {
        Vector3 targetPos = _monster.Target.transform.position;     // 타깃의 위치
        Vector3 curPos = _monster.transform.position;                       // 현재 위치
        Vector3 dirToMine = (curPos - targetPos).normalized;           // 타겟에서부터 자신까지의 방향
        Vector3 movingPos = targetPos + dirToMine * maxDistance;                     // 목표로 하는 위치
        movingPos = CheckWall(targetPos, movingPos);
        /*Vector3 dirToMP = (movingPos - curPos).normalized;           // 목표 위치로의 방향
        movingPos += dirToMP * 1f;                                 // 최종 목표 위치 (목표위치에서 +1만큼 더 가기)*/

        return movingPos;
    }

    private Vector3 CheckWall(Vector3 origin, Vector3 position)
    {
        Vector3 vecToPos = position - origin;
        Ray ray = new Ray(origin, vecToPos.normalized);
        float length = maxDistance;

        if (Physics.Raycast(ray, out RaycastHit hit, length, _monster.WallLayer))
        {
            float magnitude = hit.distance - 1.5f;

            return origin + vecToPos.normalized * (magnitude);
        }

        return position;
    }
}
