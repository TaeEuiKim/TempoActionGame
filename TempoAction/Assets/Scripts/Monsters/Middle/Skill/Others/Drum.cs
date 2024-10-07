using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour
{
    public GameObject snipingMark;
    public bool isAttack = false;

    private void OnEnable()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        isAttack = false;
    }

    private void OnDisable()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void OffMarkSet()
    {
        ObjectPool.Instance.Remove(snipingMark);
        isAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isAttack)
        {
            snipingMark.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isAttack)
        {
            snipingMark.SetActive(true);
        }
    }
}
