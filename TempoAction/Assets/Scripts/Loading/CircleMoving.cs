using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CircleMoving : MonoBehaviour
{
    [SerializeField] private LoadManager manager;
    [Header("¼ø¹ø")]
    [SerializeField] private float speed;

    RectTransform rect;
    private float moveSpeed = 0.5f;
    private float timer = 0f;

    // y += 11
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        StartCoroutine(MovingCircle());
    }

    IEnumerator MovingCircle()
    {
        yield return new WaitForSeconds(speed);

        while (manager.progressBar.fillAmount < 1.0f)
        {
            float _y = rect.position.y + 11f;
            rect.DOMoveY(rect.position.y + 11f, moveSpeed);

            yield return new WaitForSeconds(moveSpeed);

            rect.position = new Vector3(rect.position.x, _y, rect.position.z);
            _y = rect.position.y - 11f;
            rect.transform.DOMoveY(rect.position.y - 11f, moveSpeed);

            yield return new WaitForSeconds(1.5f);
        }
    }
}
