using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "Shelling", menuName = "ScriptableObjects/MiddleMonster/Skill/Shelling", order = 1)]
public class Middle_Shelling : Middle_Skill
{
    [Header("Æø¹ß ¹üÀ§")]
    [SerializeField] private Vector3 bombSize;

    [Header("Æø¹ß ´ë»ó")]
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

        if (_coolTime >= _info.coolTime) // ÄðÅ¸ÀÓ È®ÀÎ
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
        Debug.Log("ÆøÅº ÅõÇÏ");

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

    // ·ÎÄÏ ½ºÆù À§Ä¡ Y : 15
    private void SpawnCameraRocket()
    {
        for (int i = -2; i <= 2; ++i)
        {
            if (i == 0)
            {
                i++;
            }

            GameObject rocket = ObjectPool.Instance.Spawn("Rocket");
            rocket.transform.position = _monster.transform.position + new Vector3(i * 0.7f, 1);
            Vector3 moveVec = new Vector3(rocket.transform.position.x + i, 30, rocket.transform.position.z);
            rocket.transform.DOMove(moveVec, 2);
        }
    }

    private void SpawnRocket()
    {
        float _y = 25;
        Shelling rocket = ObjectPool.Instance.Spawn("Rocket").GetComponent<Shelling>();
        rocket.transform.position = new Vector3(_monster.Player.position.x + Random.Range(-3f, 3f), _y, _monster.Player.position.z);
        rocket.transform.rotation = Quaternion.Euler(-90, -200, 0);
        rocket.bombSize = bombSize;
        rocket.bombType = bombType;
        rocket.TotalDamage = Info.damage;

        GameObject mark = ObjectPool.Instance.Spawn("RocketMark");
        mark.transform.position = new Vector3(rocket.transform.position.x, 0.6f, -8f);
    }

    IEnumerator OnRocketCamera()
    {
        yield return new WaitForSeconds(2f);
        SpawnCameraRocket();

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 7; ++i)
        {
            SpawnRocket();
            yield return new WaitForSeconds(0.7f);
        }

        yield return new WaitForSeconds(3f);
        IsCompleted = false;
        _monster.FinishSkill();
    }
}
