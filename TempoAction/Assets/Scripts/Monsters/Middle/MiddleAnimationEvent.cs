using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleAnimationEvent : MonoBehaviour
{
    [SerializeField] private MiddleMonster _monster;

    private void Attack()
    {
        _monster.OnAttackAction?.Invoke();
    }


    private void Finish()
    {
        _monster.OnFinishSkill?.Invoke();
    }

    private void MoveToX()
    {
        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Vector3 rayDirection = transform.localScale.x > 0 ? transform.right : transform.right * -1;

        // ����ĳ��Ʈ ��Ʈ ���� ����
        RaycastHit hit;

        // ����ĳ��Ʈ ����
        if (!Physics.Raycast(rayOrigin, rayDirection, out hit, _monster.CurrentSkill.Info.range, _monster.WallLayer) &&
            !Physics.Raycast(rayOrigin, rayDirection, out hit, _monster.CurrentSkill.Info.range, _monster.PlayerLayer))
        {

            transform.parent.DOMoveX(transform.position.x + _monster.CurrentSkill.Info.range * rayDirection.x, 0.1f);
        }

        /*  else if (Physics.Raycast(rayOrigin, rayDirection, out hit, value * rayDirection.x, _monster.PlayerLayer))
          {
              ///transform.parent.DOMoveX(hit.point.x, 0.1f);
          }*/



        // ����׿� ���� �׸���
        Debug.DrawRay(rayOrigin, rayDirection * _monster.CurrentSkill.Info.range, Color.green);
    }
}
