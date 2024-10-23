using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject RightImg;
    [SerializeField] private GameObject LeftImg;

    private Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        if (_player.Ani.GetBool("IsCommandTime"))
        {
            if (_player.IsLeftDirection())
            {
                LeftImg.SetActive(true);
                RightImg.SetActive(false);
            }
            else
            {
                LeftImg.SetActive(false);
                RightImg.SetActive(true);
            }
        }
        else
        {
            LeftImg.SetActive(false);
            RightImg.SetActive(false);
        }
    }
}
