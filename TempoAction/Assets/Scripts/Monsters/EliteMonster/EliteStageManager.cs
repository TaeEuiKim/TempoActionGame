using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteStageManager : MonoBehaviour
{
    [SerializeField] private GameObject _phase1Monster;
    [SerializeField] private GameObject _phase2Monster;

    [SerializeField] private Define.EliteStageState _currentStageState;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeStageState(Define.EliteStageState state)
    {
        _currentStageState = state;
    }
}
