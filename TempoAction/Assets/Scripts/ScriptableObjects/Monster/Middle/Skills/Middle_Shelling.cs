using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "Shelling", menuName = "ScriptableObjects/MiddleMonster/Skill/Shelling", order = 1)]
public class Middle_Shelling : Middle_Skill
{
    [Header("몬스터에게 입히는 데미지")]
    [SerializeField] private float monsterDamage;

    [Header("폭발 범위")]
    [SerializeField] private Vector3 bombSize;

    [Header("폭발 대상")]
    [SerializeField] private LayerMask bombType;

    private float _coolTime = 0f;
    private float timer = 0f;

    public override void Init(MiddleMonster monster)
    {
        base.Init(monster);

        _coolTime = 0;
    }

    public override void Check()
    {
        if (IsCompleted) return;

        if (_coolTime >= _info.coolTime) // 쿨타임 확인
        {
            IsCompleted = true;
        }
        else
        {
            _coolTime += Time.deltaTime;
        }
    }

    public override void Enter()
    {
        Debug.Log("폭탄 투하");

        CoroutineRunner.Instance.StartCoroutine(OnRocketCamera());
    }
    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Shelling"))
        {
            _monster.Ani.SetBool("Shelling", true);
        }
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("Shelling", false);

        timer = 0f;
        _coolTime = 0;
    }

    // 로켓 스폰 위치 Y : 15
    private void SpawnRocket()
    {
        TestSound.Instance.PlaySound("Shelling_Droping");

        float _y = 23;
        Shelling rocket = ObjectPool.Instance.Spawn("Rocket").GetComponent<Shelling>();
        rocket.transform.position = new Vector3(_monster.Player.position.x, _y, _monster.Player.position.z);
        rocket.transform.rotation = Quaternion.Euler(0, 0, 0);
        rocket.bombSize = bombSize;
        rocket.bombType = bombType;
        rocket.TotalDamage = Info.damage;
        rocket.MonsterDamage = monsterDamage;
        rocket.GetComponent<Rigidbody>().velocity = Vector3.zero;

        RaycastHit ray;
        if (Physics.Raycast(rocket.transform.position, Vector3.down, out ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
        {
            GameObject mark = ObjectPool.Instance.Spawn("RocketMark");
            mark.transform.position = ray.point + new Vector3(0, 0.2f);
        }
    }

    IEnumerator OnRocketCamera()
    {
        TestSound.Instance.PlaySound("Shelling_Voice");

        yield return new WaitForSeconds(0.16f);

        TestSound.Instance.PlaySound("Shelling_Firing");

        for (int i = 0; i < 5; ++i)
        {
            SpawnRocket();

            yield return new WaitForSeconds(0.7f);
        }

        IsCompleted = false;
        _monster.FinishSkill();
    }
}
