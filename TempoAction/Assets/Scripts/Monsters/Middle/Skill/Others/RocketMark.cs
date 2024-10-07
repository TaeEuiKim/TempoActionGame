using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMark : MonoBehaviour
{
    [Header("생존 시간")]
    [SerializeField] private float time = 0f;
    private float timer = 0f;

    private void OnEnable()
    {
        timer = 0f;
        StartCoroutine(RemoveTimer());
    }

    IEnumerator RemoveTimer()
    {
        while (timer < time)
        {
            timer += Time.deltaTime;

            yield return 0;
        }

        ObjectPool.Instance.Remove(gameObject);
    }
}
