using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Launch", menuName = "ScriptableObjects/EliteMonster/Skill/Launch", order = 1)]
public class Elilte_Launch : Elite_Skill
{
    [SerializeField] private float _energyBallSpeed;
    private GameObject _energyBall;

    private float _coolTime;
    private float _totalTime;


    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
        _totalTime = 0;
    }

    public override void Check()
    {

        if (_isCompleted) return;


        if (_coolTime >= _info.coolTime)
        {
            if (Vector2.Distance(_monster.Player.position, _monster.transform.position) <= _info.range)
            {

                _coolTime = 0;
                _isCompleted = true;
            }

        }
        else
        {
            _coolTime += Time.deltaTime;
        }
    }

    public override void Enter()
    {
        Debug.Log("Åõ»çÃ¼");
        float dir = _monster.Player.transform.position.x - _monster.transform.position.x;

        _monster.Direction = dir;

        _energyBall = ObjectPool.Instance.Spawn("EnergyBall");
        _energyBall.transform.position = _monster.StartEnergyBallPoint.position;

        _energyBall.GetComponent<EnergyBall>().totalDamage = _monster.Stat.AttackDamage * (_info.damage / 100);

    }
    public override void Stay()
    {
        _energyBall.transform.Translate(new Vector2(_monster.Direction, 0) * _energyBallSpeed * Time.deltaTime);

        if (_totalTime >= _info.totalTime)
        {
            _monster.CurrentSkill = null;
        }
        else
        {
            _totalTime += Time.deltaTime;
        }

    }
    public override void Exit()
    {
        ObjectPool.Instance.Remove(_energyBall);
        _totalTime = 0;
        _monster.ResetSkill();
        _isCompleted = false;
    }
}
