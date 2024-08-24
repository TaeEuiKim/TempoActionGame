using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Launch", menuName = "ScriptableObjects/EliteMonster/Skill/Launch", order = 1)]
public class Elilte_Launch : Elite_Skill
{
    private float _coolTime;
    

    [SerializeField] private float _energyBallSpeed;
    private GameObject _energyBall;

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
    }

    public override bool Check()
    {
        if (_coolTime >= _info.coolTime) // 쿨타임 확인
        {
            if (Vector2.Distance(_monster.Player.position, _monster.transform.position) <= _info.range) // 거리 확인
            {              
                return true;
            }
        }
        else
        {
            _coolTime += Time.deltaTime;
        }

        return false;
    }

    public override void Enter()
    {
        Debug.Log("투사체");
        _monster.Direction = _monster.Player.transform.position.x - _monster.transform.position.x; // 플레이어 바라보기

       

        CoroutineRunner.Instance.StartCoroutine(ExcuteLaunch());
    }
    public override void Stay()
    {
       

    }
    public override void Exit()
    {
        
        if (_energyBall.activeSelf)
        {
            GameObject explosion = ObjectPool.Instance.Spawn("ElectricBallExplosion");
            explosion.transform.position = _energyBall.transform.position;
            ObjectPool.Instance.Remove(_energyBall);
        }
        
        _coolTime = 0;
    }

    private IEnumerator ExcuteLaunch()
    {
        _monster.Ani.SetBool("Launch", true);

        yield return new WaitForSeconds(0.3f);

        _energyBall = ObjectPool.Instance.Spawn("ElectricBall");
        _energyBall.transform.SetParent(_monster.StartEnergyBallPoint);
        _energyBall.transform.localPosition = Vector3.zero;
        _energyBall.GetComponent<ElectricBall>().TotalDamage = _monster.Stat.Damage * (_info.damage / 100);

        yield return new WaitForSeconds(0.5f);
        _energyBall.transform.SetParent(null);

        float totalTime = 0;
        while (totalTime < _info.totalTime)
        {
            _energyBall.transform.Translate(new Vector2(_monster.Direction, 0) * _energyBallSpeed * Time.deltaTime);
            totalTime += Time.deltaTime;

            yield return null;
        }

        _monster.Ani.SetBool("Launch", false);
        _monster.FinishSkill();
    }
}
