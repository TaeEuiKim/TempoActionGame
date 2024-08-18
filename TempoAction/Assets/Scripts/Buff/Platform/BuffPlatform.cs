using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuffPlatform : MonoBehaviour
{
    [SerializeField] private Define.BuffInfo _info = Define.BuffInfo.NONE;

    private BuffData _buffData;

    private bool _isEntered = false;


    public void Change(Define.BuffInfo info)
    {
       
        Exit();

        _info = info;

        Material temp;
        if (_info == Define.BuffInfo.NONE)
        {
            temp = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            temp.color = Color.white;         
        }
        else
        {
            _buffData = BuffManager.Instance.FindBuff(_info);
            _buffData.Platform = this;

            temp = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            temp.color = _buffData.color;
        }

        if (Application.isPlaying)
        {
            GetComponent<Renderer>().material = temp;
        }
        else
        {
            GetComponent<Renderer>().sharedMaterial = temp;
        }
    }

    public void ChangeColor(Define.BuffInfo info)
    {
        Material temp;
        if (info == Define.BuffInfo.NONE)
        {
            temp = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            temp.color = Color.white;
        }
        else
        {
            temp = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            temp.color = BuffManager.Instance.FindBuff(info).color;
        }

        if (Application.isPlaying)
        {
            GetComponent<Renderer>().material = temp;
        }
        else
        {
            GetComponent<Renderer>().sharedMaterial = temp;
        }
    }


    private void Enter()
    {
        if (!_isEntered)
        {
            if (_info == Define.BuffInfo.NONE) return;

            if (BuffManager.Instance.FindBuff(_info).type != Define.BuffType.EXIT)
            {
                BuffManager.Instance.AddBuff(_info);
            }
            //print("들어감");
            _isEntered = true;
        }
    }

    private void Exit()
    {
        if (_isEntered)
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
            _isEntered = false;
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
