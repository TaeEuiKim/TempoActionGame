using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "Shelling", menuName = "ScriptableObjects/MiddleMonster/Skill/Shelling", order = 1)]
public class Middle_Shelling : Middle_Skill
{
    public float cameraSpeed = 0f;

    private float _coolTime = 0f;
    private CinemachineVirtualCamera _virtualCamera;
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

        _monster.transform.DOMove(_monster.middlePoint[Define.MiddleMonsterPoint.SHELLINGPOINT].position, 2);
        _virtualCamera = _monster.GetComponentInChildren<CinemachineVirtualCamera>(true);
        _virtualCamera.gameObject.SetActive(true);

        CoroutineRunner.Instance.StartCoroutine(OnRocketCamera());
    }
    public override void Stay()
    {

    }

    public override void Exit()
    {
        timer = 0f;
        _coolTime = 0;
        _virtualCamera = null;
    }

    // 로켓 스폰 위치 Y : 15
    private void SpawnCameraRocket()
    {
        for (int i = -2; i <= 2; ++i)
        {
            if (i == 0)
            {
                i++;
            }

            GameObject rocket = ObjectPool.Instance.Spawn("Rocket");
            rocket.transform.position = _monster.transform.position + new Vector3(i, 1);
            rocket.transform.DOMoveY(30, 6);
        }
    }

    private void SpawnRocket()
    {
        float _y = 25;
        GameObject rocket = ObjectPool.Instance.Spawn("Rocket");
        rocket.transform.position = new Vector3(_monster.Player.position.x, _y, _monster.Player.position.z);
        GameObject mark = ObjectPool.Instance.Spawn("RocketMark");
        mark.transform.position = new Vector3(rocket.transform.position.x, 0.6f, -8f);
        mark.transform.rotation = Quaternion.Euler(90, 0, 0);
        mark.GetComponent<Mark>().rocket = rocket;

        rocket.transform.DOMoveY(-10, 3);

        for (int i = 0; i < 3; ++i)
        {
            rocket = ObjectPool.Instance.Spawn("Rocket");
            float _x = Random.Range(_monster.middlePoint[Define.MiddleMonsterPoint.BOMBLEFTPOINT].position.x,
                         _monster.middlePoint[Define.MiddleMonsterPoint.BOMBRIGHTPOINT].position.x);
            rocket.transform.position = new Vector3(_x, _y, _monster.middlePoint[Define.MiddleMonsterPoint.BOMBLEFTPOINT].position.z);
            mark = ObjectPool.Instance.Spawn("RocketMark");
            mark.transform.position = new Vector3(_x, 0.6f, -8f);
            mark.transform.rotation = Quaternion.Euler(90, 0, 0);
            mark.GetComponent<Mark>().rocket = rocket;

            rocket.transform.DOMoveY(-10, 3);
        }
    }

    IEnumerator OnRocketCamera()
    {
        yield return new WaitForSeconds(2f);
        CinemachineComposer composer = _virtualCamera.GetCinemachineComponent<CinemachineComposer>();
        SpawnCameraRocket();

        while (composer.m_TrackedObjectOffset.y < 4) 
        {
            composer.m_TrackedObjectOffset.y += Time.deltaTime * cameraSpeed;

            yield return new WaitForSeconds(0.001f);
        }

        _virtualCamera.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        SpawnRocket();

        yield return new WaitForSeconds(5f);
        SpawnRocket();

        yield return new WaitForSeconds(3f);
        _monster.transform.DOMove(_monster.middlePoint[Define.MiddleMonsterPoint.GSPAWNPOINT].position, 2f);

        yield return new WaitForSeconds(2f);
        IsCompleted = false;
        _monster.FinishSkill();
    }
}
