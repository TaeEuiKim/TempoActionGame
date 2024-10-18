using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Normal_Death : Normal_State
{
    private bool isItem = false;

    public Normal_Death(NormalMonster monster) : base(monster)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _monster.GetComponent<BoxCollider>().enabled = false;
        _monster.Rb.useGravity = false;
        _monster.CurrentSkillSlots = null;

        if (!_monster.Ani.GetBool("Death"))
        {
            _monster.Ani.SetBool("Death", true);
        }

        if (!isItem)
        {
            SpawnItem();
        }
    }

    public override void Stay()
    {
        Debug.LogError(_monster.Ani.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (_monster.Ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f &&
            _monster.Ani.GetCurrentAnimatorStateInfo(0).IsName("AC_BaldoMon_Death"))
        {
            ObjectPool.Instance.Remove(_monster.gameObject);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void SpawnItem()
    {
        isItem = true;

        GameObject sword = ObjectPool.Instance.Spawn("Sword");
        sword.transform.position = _monster.transform.position + new Vector3(0, 3f);
        NormalSkill sw = new NormalSkill(_monster.skillData);

        var so = sword.GetComponent<SkillObject>();
        so.Initialize(sw);
    }
}