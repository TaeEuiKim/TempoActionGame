using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class EliteMonsterAttack : MonoBehaviour
{
    [SerializeField] private EliteMonster _monster;

    private void Hit()
    {
        _monster.OnHitAction?.Invoke();
    }


    private void Finish()
    {
        _monster.OnFinishSkill?.Invoke();
    }

    private void DoMoveX(float value)
    {

    }
}
