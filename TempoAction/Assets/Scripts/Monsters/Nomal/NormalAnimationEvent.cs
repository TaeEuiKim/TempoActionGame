using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        }
    }

    private void Finish()
    {
        if (!_monster.TrySkillAttack())
        {
            _monster.Ani.SetBool("Hit", false);
            _monster.CurrentPerceptionState = Define.PerceptionType.IDLE;
            _monster.StartHitTimer();
        }
    }

    private void RunStopFinish()
    {
        _monster.Ani.SetBool("RunStop", false);
    }
}
