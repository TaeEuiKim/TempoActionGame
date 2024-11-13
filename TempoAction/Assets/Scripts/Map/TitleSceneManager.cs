using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    void Start()
    {
        TestSound.Instance.PlaySound("TitleBGM");
    }
}
