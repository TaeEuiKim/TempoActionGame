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
        _monster.GetComponent<BoxCollider>().enabled = false;
        _monster.Rb.useGravity = false;

        if (!_monster.Ani.GetBool("Death"))
        {
            _monster.Ani.SetBool("Death", true);
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

        GameObject ultimateEffect = ObjectPool.Instance.Spawn("energy_ui_ball");
        _monster.Player.GetComponent<Player>().View.MoveUltimateUI(ultimateEffect, _monster.ultimateValue / 100);
    }
}