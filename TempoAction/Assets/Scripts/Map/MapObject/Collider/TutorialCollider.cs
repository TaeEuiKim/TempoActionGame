using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    [Header("Æ©Åä¸®¾ó ¿ë UI")]
    [SerializeField] private GameObject[] ui;
    [Header("Bool")]
    [SerializeField] private bool isTurn;

    StartSceneManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<StartSceneManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!isTurn)
            {
                for (int i = 0; i < ui.Length; ++i)
                {
                    ui[i].SetActive(false);
                }

                GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                for (int i = 0; i < ui.Length; ++i)
                {
                    ui[i].SetActive(true);
                }

                manager.SetPlayerControll(true, true);
                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
