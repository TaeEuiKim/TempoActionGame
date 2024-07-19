using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TempoCircle : MonoBehaviour
{
    private PlayerManager _player;

    [SerializeField] private Image _circleImage; // 원 스프라이트 이미지
    [SerializeField] private float _shrinkDuration = 1f; // 원이 줄어드는데 걸리는 시간 (초)
    [SerializeField] private Vector2 _perfectTime; // 완벽한 타이밍 (초)
    [SerializeField] private Vector2 _goodTime; // 좋은 타이밍 (초)
    [SerializeField] private TextMeshProUGUI checkText;

    private float timer = 0f;
    private bool isShrinking = true;


    void Start()
    {       
        ResetCircle();
    }

    void Update()
    {
        if (isShrinking)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(1.0f, 0.3f, timer / _shrinkDuration);
            _circleImage.transform.localScale = new Vector3(scale, scale, 1.0f);

            if (timer >= _shrinkDuration)
            {
                _player.Atk.CircleState = Define.CircleState.BAD;
                checkText.text = "Bad";
                //Debug.Log("Bad!");
                isShrinking = false;
                _circleImage.gameObject.SetActive(false);
                Invoke("Finish", 0.5f);
            }
            else
            {
                if (InputManager.Instance.GetInpuState("PointTempo") == Define.InputState.DOWN)
                {
                    CheckTiming();
                    Invoke("Finish", 0.5f);
                }
            }        
        }
    }

    private void CheckTiming()
    {
        float timeLeft = _shrinkDuration - timer;
        if (_perfectTime.x <= timeLeft && timeLeft < _perfectTime.y)
        {
            _player.Atk.CircleState = Define.CircleState.PERFECT;
            checkText.text = "Perfect";
            //Debug.Log("Perfect!");
            isShrinking = false;
        }
        else if (_goodTime.x <= timeLeft && timeLeft < _goodTime.y)
        {
            _player.Atk.CircleState = Define.CircleState.GOOD;
            checkText.text = "Good";
            //Debug.Log("Good!");
            isShrinking = false;
        }
        else if(_goodTime.y < timeLeft || _perfectTime.x < timeLeft )
        {
            _player.Atk.CircleState = Define.CircleState.BAD;
            checkText.text = "Bad";
            //Debug.Log("Bad!");
            isShrinking = false;
        }
    }

    public void ResetCircle()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerManager>();
        }       

        timer = 0.0f;
        _player.Atk.CircleState = Define.CircleState.NONE;
        _circleImage.gameObject.SetActive(true);
        checkText.text = "";
        _circleImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        isShrinking = true;
    }

    private void Finish()
    {
        gameObject.SetActive(false);
    }
}
