using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_NormalAttackState : Normal_AttackState
{
    public Normal_NormalAttackState(NormalMonster monster) : base(monster) { }

    public override void Enter()
    {
        base.Enter();

        if (!_monster.Ani.GetBool("Attack"))
        {
            _monster.Ani.SetBool("Attack", true);
        }
        _monster.isAttack = false;
    }

    public override void Exit()
    {
        base.Exit();

        CoroutineRunner.Instance.StartCoroutine(CheckAttackTimer());
        _monster.Ani.SetBool("Attack", false);
    }

    private IEnumerator CheckAttackTimer()
    {
        float attackTimer = 0f;

        while (!_monster.isAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > _monster.MonsterSt.AttackDelay)
            {
                attackTimer = 0f;
                _monster.isAttack = true;
            }

            yield return new WaitForSeconds(0.02f);
        }
    }
}
