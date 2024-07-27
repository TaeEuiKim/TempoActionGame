using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPlatform : MonoBehaviour
{
    [SerializeField] private Define.BuffInfo _info;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (BuffManager.Instance.FindBuff(_info).type != Define.BuffType.EXIT)
            {
                BuffManager.Instance.AddBuff(_info);
            }
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (BuffManager.Instance.FindBuff(_info).type == Define.BuffType.EXIT)
            {
                BuffManager.Instance.AddBuff(_info);
            }
            else if (BuffManager.Instance.FindBuff(_info).type == Define.BuffType.STAY)
            {
                BuffManager.Instance.RemoveBuff(_info);
            }
        }
      
    }
}
