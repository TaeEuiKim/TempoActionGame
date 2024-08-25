using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterAttack : MonoBehaviour
{
    [SerializeField] private EliteMonster _monster;

    private void Attack()
    {
        _monster.OnHitAction?.Invoke();
    }

    private void Finish()
    {
        _monster.OnFinishSkill?.Invoke();
    }
}
