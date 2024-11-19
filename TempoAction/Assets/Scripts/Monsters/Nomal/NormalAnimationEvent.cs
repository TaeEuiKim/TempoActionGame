using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAnimationEvent : MonoBehaviour
{
    [SerializeField] private NormalMonster _monster;

    private void Attack()
    {
        bool isHit = Physics.CheckBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        if (isHit)
        {
            var player = _monster.Player.GetComponent<Player>();
            player.TakeDamage(_monster.Stat.Damage);

            if (_monster.monsterType == Define.NormalMonsterType.BALDO)
            {
                TestSound.Instance.PlaySound("NormalMonster1_Attack1_Hit");
            }

            if (_monster.monsterType == Define.NormalMonsterType.MON3)
            {
                GameObject effect;
                if (_monster.Ani.GetInteger("AttackCount") == 0)
                {
                    TestSound.Instance.PlaySound("NormalMonster3_Hit");
                    effect = ObjectPool.Instance.Spawn("P_Monster3Hit1", 1f);
                    effect.transform.position = _monster.Player.position + new Vector3(0, 1.2f);
                }
                else
                {
                    TestSound.Instance.PlaySound("NormalMonster3_Hit");
                    effect = ObjectPool.Instance.Spawn("P_Monster3Hit2", 1f);
                    effect.transform.position = _monster.Player.position + new Vector3(0, 1.2f);
                }
            }
        }
    }

    private void Finish()
    {
        if (!_monster.GetSkillAttackUsable())
        {
            _monster.Ani.SetBool("Hit", false);
            _monster.CurrentPerceptionState = Define.PerceptionType.IDLE;
            _monster.StartHitTimer();
        }
    }

    private void HitFinish()
    {
        _monster.isHit = false;
        _monster.isHiting = false;
        _monster.Ani.SetBool("Hit", false);
        _monster.CurrentPerceptionState = Define.PerceptionType.IDLE;
    }

    private void RunStopFinish()
    {
        _monster.Ani.SetBool("RunStop", false);
    }

    private void SetEffectMonster(string name)
    {
        GameObject effect;
        int dir = 1;
        switch (name)
        {
            case "P_BaldoSlash1_Attack1_1":
                effect = ObjectPool.Instance.Spawn(name, 1f);
                if (_monster.IsLeftDirection())
                {
                    if (_monster.monsterType == Define.NormalMonsterType.KUNG)
                    {
                        effect.transform.localScale = new Vector3(1, 1, -1);
                    }
                    else
                    {
                        effect.transform.localScale = new Vector3(-1, 1, -1);
                    }

                    dir = -1;
                }
                else
                {
                    if (_monster.monsterType == Define.NormalMonsterType.KUNG)
                    {
                        effect.transform.localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        effect.transform.localScale = new Vector3(-1, 1, 1);
                    }
                }
                effect.transform.position = _monster.transform.position + new Vector3(0.8f * dir, 1f);
                break;
            case "P_BaldoSlash1_Attack1_2":
                effect = ObjectPool.Instance.Spawn(name, 1f);
                if (_monster.IsLeftDirection())
                {
                    effect.transform.localScale = new Vector3(1, 1, -1);
                    dir = -1;
                }
                else
                {
                    effect.transform.localScale = new Vector3(1, 1, 1);
                }
                effect.transform.position = _monster.transform.position + new Vector3(0.8f * dir, 1f);
                break;
            case "P_BaldoSlash1_Attack1_3":
                effect = ObjectPool.Instance.Spawn(name, 1f);
                if (_monster.IsLeftDirection())
                {
                    effect.transform.localScale = new Vector3(1, 1, -1);
                    dir = -1;
                }
                else
                {
                    effect.transform.localScale = new Vector3(1, 1, 1);
                }
                effect.transform.position = _monster.transform.position + new Vector3(0.8f * dir, 1f);
                break;
            case "P_Monster3Attack1":
                dir = _monster.IsLeftDirection() ? -1 : 1;
                effect = ObjectPool.Instance.Spawn(name, 1f);
                effect.transform.position = _monster.transform.position + new Vector3(0.5f * dir, 1.4f);
                effect.transform.localScale = new Vector3(dir, 1, 1);
                break;
            case "P_Monster3Attack2":
                dir = _monster.IsLeftDirection() ? -1 : 1;
                if (dir == -1)
                {
                    effect = ObjectPool.Instance.Spawn("P_Monster3Attack2Left", 1f);
                }
                else
                {
                    effect = ObjectPool.Instance.Spawn("P_Monster3Attack2Right", 1f);
                }
                effect.transform.position = _monster.transform.position + new Vector3(0, 1.8f);
                break;
            default:
                break;
        }
    }

    private void PlaySound(string name)
    {
        TestSound.Instance.PlaySound(name);
    }
}
