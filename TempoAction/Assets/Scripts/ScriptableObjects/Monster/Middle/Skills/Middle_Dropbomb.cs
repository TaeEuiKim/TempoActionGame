using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Dropbomb", menuName = "ScriptableObjects/MiddleMonster/Skill/Dropbomb", order = 1)]
public class Middle_Dropbomb : Middle_Skill
{
    [SerializeField] private int bombAmount;        // 폭탄 개수
    [SerializeField] private float launchAngle = 45f;     // 발사 각도

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

        if (_coolTime >= _info.coolTime) // 쿨타임 확인
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
        Debug.Log("유도 미사일 발사");

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

    private void LaunchProjectile(GameObject bomb)
    {
        Vector3 targetPos = _monster.Player.transform.position + new Vector3(0, 0, 4f);
        Vector3 startPos = _monster.transform.position + new Vector3(0, 0, -1f);

        float distance = Vector3.Distance(targetPos, startPos);

        // 높이 차이 계산
        float heightDifference = targetPos.y - startPos.y;

        // 수평 거리와 발사 각도를 사용한 속도 계산
        float angleInRadians = launchAngle * Mathf.Deg2Rad;
        float horizontalDistance = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z).magnitude;

        // 발사 속도 계산 (물리 공식 사용)
        float velocity = Mathf.Sqrt(horizontalDistance * Physics.gravity.magnitude / Mathf.Sin(2 * angleInRadians));

        // 방향 벡터 계산
        Vector3 direction = (targetPos - startPos).normalized;

        // 힘 가하기 (3D 공간에서)
        Vector3 velocityVector = direction * velocity * Mathf.Cos(angleInRadians);
        velocityVector.y = velocity * Mathf.Sin(angleInRadians);

        bomb.GetComponent<Rigidbody>().velocity = velocityVector;  // Rigidbody에 속도 적용
    }

    private void Attack()
    {
        GameObject bomb = ObjectPool.Instance.Spawn("TraceRocket");
        bomb.transform.position = _monster.HitPoint.position + new Vector3(0, 0, -1f);

        LaunchProjectile(bomb);
    }

    private void Finish()
    {
        IsCompleted = false;
        _monster.FinishSkill();
    }
}
