using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPlatform : MonoBehaviour
{
    [SerializeField] private Define.BuffInfo _info = Define.BuffInfo.NONE;
    private BuffData _buffData;

    private bool isEntered = false;


    public void Change(Define.BuffInfo info)
    {
        Exit();

        _info = info;

        if (_info == Define.BuffInfo.NONE)
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
        else
        {
            _buffData = BuffManager.Instance.FindBuff(_info);
            _buffData.Platform = this;
            GetComponent<Renderer>().material.color = _buffData.color;

        }
    }

    private void Enter()
    {
        if (!isEntered)
        {
            if (_info == Define.BuffInfo.NONE) return;

            if (BuffManager.Instance.FindBuff(_info).type != Define.BuffType.EXIT)
            {
                BuffManager.Instance.AddBuff(_info);
            }
            //print("들어감");
            isEntered = true;
        }
    }

    private void Exit()
    {
        if (isEntered)
        {
            if (_info == Define.BuffInfo.NONE) return;

            if (BuffManager.Instance.FindBuff(_info).type == Define.BuffType.EXIT)
            {
                BuffManager.Instance.AddBuff(_info);
            }
            else if (BuffManager.Instance.FindBuff(_info).type == Define.BuffType.STAY)
            {
                BuffManager.Instance.RemoveBuff(_info);
            }
            //print("나감");
            isEntered = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Enter();

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Exit();

        }

    }
}
