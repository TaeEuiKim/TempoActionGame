using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleAnimationEvent : MonoBehaviour
{
    [SerializeField] private MiddleMonster _monster;

    [Header("경채 총구")]
    [SerializeField] private Transform gun;

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

        // 레이캐스트 히트 정보 저장
        RaycastHit hit;

        // 레이캐스트 실행
        if (!Physics.Raycast(rayOrigin, rayDirection, out hit, _monster.CurrentSkill.Info.range, _monster.WallLayer) &&
            !Physics.Raycast(rayOrigin, rayDirection, out hit, _monster.CurrentSkill.Info.range, _monster.PlayerLayer))
        {

            transform.parent.DOMoveX(transform.position.x + _monster.CurrentSkill.Info.range * rayDirection.x, 0.1f);
        }

        /*  else if (Physics.Raycast(rayOrigin, rayDirection, out hit, value * rayDirection.x, _monster.PlayerLayer))
          {
              ///transform.parent.DOMoveX(hit.point.x, 0.1f);
          }*/
    }

    private void SetGyeongChaeAimCount(int count)
    {
        _monster.Ani.SetInteger("AimAttackCount", count);
    }

    private void SetGyeongChaeEffect(int count)
    {
        if (count == 0)
        {
            GameObject explosion = ObjectPool.Instance.Spawn("fung_gyung", 1);

            explosion.transform.position = gun.transform.position;
        }
        else if (count == 1)
        {
            GameObject smoke = ObjectPool.Instance.Spawn("gyung_smoke", 1);

            smoke.transform.position = gun.transform.position;
        }
    }

    private void GyeongChaeSpawnRocket()
    {
        GameObject rocket = ObjectPool.Instance.Spawn("Rocket");
        rocket.transform.position = gun.transform.position + new Vector3(0, 0.3f, -0.8f);
        rocket.transform.rotation = Quaternion.Euler(140, 0, 0);
        Vector3 moveVec = new Vector3(rocket.transform.position.x, 30, rocket.transform.position.z - 3f);
        rocket.transform.DOMove(moveVec, 3.5f);
    }
}
