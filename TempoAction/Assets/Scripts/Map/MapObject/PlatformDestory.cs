using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestory : MonoBehaviour
{
    [Header("파괴 까지 걸리는 시간")]
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
