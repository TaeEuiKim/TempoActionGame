using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Barrier", menuName = "ScriptableObjects/EliteMonster/Skill/Barrier", order = 1)]
public class Elite_Barrier : Elite_Skill
{
    private float _coolTime;
    private float _totalTime;

    [SerializeField] private float _defense;   // 데미지 감소량

    private float _lastHp;

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
        _totalTime = 0;
    }

    public override bool Check()
    {
        if (_coolTime >= _info.coolTime) // 쿨타임 확인
        {          
            return true;
        }
        else
        {
            _coolTime += Time.deltaTime;
        }

        return false;
    }

    public override void Enter()
    {
        Debug.Log("가드");
        _lastHp = _monster.Stat.Health;
        _monster.Stat.Defense = _defense;
    }
    public override void Stay()
    {
        if (_totalTime >= _info.totalTime)
        {
            Debug.Log(_lastHp +" / "+ _monster.Stat.Health);
            if (_lastHp > _monster.Stat.Health) // 플레이어가 가드 상태인 몬스터 공격 시(체력 변화가 있을 때)
            {
                _monster.ChangeCurrentSkill(Define.EliteMonsterSkill.SUPERPUNCH); // 멀리 치기 실행
            }
            else
            {
                _monster.FinishSkill();
            }
            Debug.Log("가드 끝");
        }
        else
        {
            _totalTime += Time.deltaTime;
        }
    }
    public override void Exit()
    {
        _monster.Stat.Defense = 0;
        _totalTime = 0;
        _coolTime = 0;
    }

}
