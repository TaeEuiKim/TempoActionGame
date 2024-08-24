using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TempoCircle : MonoBehaviour
{
    private Transform _player;

    [SerializeField] private GameObject _checkCircle; // 원 스프라이트 이미지
    [SerializeField] private GameObject _perfectCircle; // 원 스프라이트 이미지
    [SerializeField] private GameObject _goodCircle; // 원 스프라이트 이미지
    [SerializeField] private float _shrinkDuration = 1f; // 원이 줄어드는데 걸리는 시간 (초)
    

    [SerializeField] private Vector2 _perfectTime; // 완벽한 타이밍 (초)
    [SerializeField] private Vector2 _goodTime; // 좋은 타이밍 (초)
    private Vector2 _perfectScale; // 완벽한 타이밍 (초)
    private Vector2 _goodScale; // 좋은 타이밍 (초)

    [Space]
    [SerializeField] private GameObject _perfectPrefab;
    [SerializeField] private GameObject _goodPrefab;
    [SerializeField] private GameObject _badPrefab;
    [SerializeField] private GameObject _missPrefab;


    private float timer = 0f;
    [SerializeField] private bool isShrinking = true; //축소 중 확인

    private Define.CircleState _circleState = Define.CircleState.NONE;

    public Action OnSuccess;
    public Action OnFailure;
    public Action OnFinish;

    public float ShrinkDuration { get => _shrinkDuration; set => _shrinkDuration = value; }
    public Define.CircleState CircleState { get => _circleState; }

    public bool IsAvailable { get; set; } = true;

    void Update()
    {
        if (isShrinking)
        {
            if (timer >= _shrinkDuration)
            {
                //Debug.Log("Miss!");
                _circleState = Define.CircleState.MISS;
                
                isShrinking = false;

                _checkCircle.SetActive(false);

                SpawnFx(_circleState);

                Finish();
                //Invoke("Finish", 0.5f);              
            }
            else
            {
                timer += Time.deltaTime;

                float scale = Mathf.Lerp(1.0f, 0, timer / _shrinkDuration);
                _checkCircle.transform.localScale = new Vector3(scale, scale, 1.0f);

                if (_player.GetComponent<Player>().CurrentState != Define.PlayerState.STUN && IsAvailable) // 스턴 상태면 입력 안되도록
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        CheckTiming();

                        isShrinking = false;

                        Finish();
                        //Invoke("Finish", 0.5f);
                    }

                }
            }        
        }
    }

    // 초기화 함수
    public void Init(Transform player = null)
    {
        timer = 0.0f;
        _shrinkDuration = 1;


        _checkCircle.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        _checkCircle.SetActive(true);

        _perfectScale.x = Mathf.Lerp(0, 1, _perfectTime.x / _shrinkDuration);
        _perfectScale.y = Mathf.Lerp(0, 1, _perfectTime.y / _shrinkDuration);
        _perfectCircle.transform.localScale = new Vector3(_perfectScale.x, _perfectScale.x, _perfectScale.x); ;

        _goodScale.x = Mathf.Lerp(0, 1, _goodTime.x / _shrinkDuration);
        _goodScale.y = Mathf.Lerp(0, 1, _goodTime.y / _shrinkDuration);
        _goodCircle.transform.localScale = new Vector3(_goodScale.x, _goodScale.x, _goodScale.x); 

        isShrinking = true;
        _circleState = Define.CircleState.NONE;

        _player = player;

        IsAvailable = true;
    }

    // 타이밍 확인 함수
    private void CheckTiming()
    {
        if (_perfectScale.x <= _checkCircle.transform.localScale.x && _checkCircle.transform.localScale.x < _perfectScale.y)
        {
            _circleState = Define.CircleState.PERFECT;
            //Debug.Log("Perfect!");
        }
        else if (_goodScale.x <= _checkCircle.transform.localScale.x && _checkCircle.transform.localScale.x < _goodScale.y)
        {
            _circleState = Define.CircleState.GOOD;

            //Debug.Log("Good!");
        }
        else if(_goodTime.y < _checkCircle.transform.localScale.x || _perfectScale.x > _checkCircle.transform.localScale.x)
        {
            _circleState = Define.CircleState.BAD;
            //Debug.Log("Bad!");
        }

        SpawnFx(_circleState);
       
    }

    // 이펙트 생성 함수
    private void SpawnFx(Define.CircleState state)
    {      
        GameObject temp = null;

        switch (state)
        {
            case Define.CircleState.PERFECT:
                temp = Instantiate(_perfectPrefab, _player.position + new Vector3(0, 1, 0), Quaternion.identity);
                OnSuccess?.Invoke();
                break;
            case Define.CircleState.GOOD:
                temp = Instantiate(_goodPrefab, _player.position + new Vector3(0, 1, 0), Quaternion.identity);
                OnSuccess?.Invoke();
                break;
            case Define.CircleState.BAD:
                temp = Instantiate(_badPrefab, _player.position + new Vector3(0, 1, 0), Quaternion.identity);
                OnFailure?.Invoke();
                break;
            case Define.CircleState.MISS:
                temp = Instantiate(_missPrefab, _player.position + new Vector3(0, 1, 0), Quaternion.identity);
                OnFailure?.Invoke();
                break;
        }
      
        Destroy(temp, 1f);

    }

    private void Finish()
    {      
        ObjectPool.Instance.Remove(gameObject);
        OnFinish?.Invoke();
        
        OnSuccess = null;
        OnFailure = null;
        OnFinish = null;
    }
}
