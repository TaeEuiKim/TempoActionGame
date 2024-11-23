using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Normal_Death : Normal_State
{
    private bool isSpawn = false;

    public Normal_Death(NormalMonster monster) : base(monster)
    {

    }

    public override void Enter()
    {
        base.Enter();

        _monster.Rb.velocity = Vector3.zero;

        if (!_monster.Ani.GetBool("Death"))
        {
            _monster.Ani.SetBool("Death", true);
        }

        if (_monster.monsterType == Define.NormalMonsterType.MON3)
        {
            int num = Random.Range(0, 2);
            if (num == 0)
            {
                TestSound.Instance.PlaySound("NormalMonster3_Dead1");
            }
            else
            {
                TestSound.Instance.PlaySound("NormalMonster3_Dead2");
            }
        }

        if (!isSpawn)
        {
            SpawnUltimateGauge();
        }
    }

    public override void Stay()
    {
        if (_monster.Ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
            && _monster.Ani.GetCurrentAnimatorStateInfo(0).IsTag("Death"))
        {
            ObjectPool.Instance.Remove(_monster.gameObject);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void SpawnUltimateGauge()
    {
        isSpawn = true;

        var player = _monster.Player.GetComponent<Player>();

        player.View.MoveUltimateUI(_monster.ultimateValue / _monster.Player.GetComponent<Player>().PlayerSt._maxUltimateGauge);
        player.SetKillCount();

        if (_monster.monsterType == Define.NormalMonsterType.MON3)
        {
            GameObject food = ObjectPool.Instance.Spawn("Dumplings");
            food.transform.position = _monster.transform.position + new Vector3(0, 0.5f);
        }
    }
}