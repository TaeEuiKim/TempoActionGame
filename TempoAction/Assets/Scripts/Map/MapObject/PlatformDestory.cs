using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestory : MonoBehaviour
{
    [Header("�ı� ���� �ɸ��� �ð�")]
    [SerializeField] float destoryTime = 0.5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(DestoryPlatform());
        }
    }

    IEnumerator DestoryPlatform()
    {
        yield return new WaitForSeconds(destoryTime);

        GetComponent<Collider>().enabled = false;
    }
}
