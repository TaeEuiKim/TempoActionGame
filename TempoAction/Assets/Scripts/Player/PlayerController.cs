using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerController
{
    private Player _player;

    [Header("키 입력 기록 리스트")]
    private List<KeyCode> keyInputs = new List<KeyCode>();
    [Header("키 입력 시간 제한")]
    private float inputTimeLimit = 0.2f;
    private float lastInputTime;

    private bool _isLanded;
    private bool _isGrounded;
    private bool _isOnMonster;
    private bool _isDashing;
    private bool _isDoubleJumping;
    public bool isDown = false;

    private float _dashTimer;
    protected float _direction;
    protected float _dashDirection;
    public float Direction
    {
        get => _direction;
        set
        {
            if (value > 0)
            {
                value = 1;
            }
            else if (value < 0)
            {
                value = -1;
            }

            if (_direction != value)
            {
                Flip(value);
            }

            _direction = value;
        }
    }

    public PlayerController(Player player)
    {
        _player = player;
    }

    public void Initialize()
    {
        _isLanded = false;
        _isGrounded = true;
        _isDashing = false;

        _dashDirection = 1;
        _dashTimer = 0f;

        _dashTimer = _player.PlayerSt.DashDelay;
    }

    public void Update()
    {
        if (_player.PlayerSt.IsKnockedBack) return;

        if (_player.Attack.CurrentAttackkState == Define.AttackState.ATTACK)
        {
            Stop();

            return;
        }

        if (!(_isGrounded = Physics.CheckSphere(_player.GroundCheckPoint.position, _player.GroundCheckRadius, _player.GroundLayer)))
        {
            _isGrounded = Physics.CheckSphere(_player.GroundCheckPoint.position, _player.GroundCheckRadius, _player.WallLayer);
        }
        _isOnMonster = Physics.CheckSphere(_player.GroundCheckPoint.position, _player.GroundCheckRadius, _player.MonsterLayer);
        _player.Ani.SetBool("isGrounded", _isGrounded);

        if (_isGrounded)
        {
            if (!_isLanded)
            {
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_JumpLanding", _player.transform);
                _isDoubleJumping = false;
                _isLanded = true;
            }
        }
        else
        {
            _isLanded = false;
        }

        if (_isOnMonster)
        {
            //Vector3 force = new Vector3(-_player.CharacterModel.localScale.x * 10f, -5f);
            //Debug.Log(force);
            //_player.Rb.AddForce(force, ForceMode.VelocityChange);
        }

        if (_dashTimer >= _player.PlayerSt.DashDelay)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RecordInput(KeyCode.LeftArrow);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RecordInput(KeyCode.RightArrow);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C) || CheckDash())
            {
                Dash();
            }
        }
        else
        {
            _dashTimer += Time.deltaTime;
        }


        if (!_isDashing)
        {
            Move();
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && _isGrounded)
        {
            isDown = true;
        }
    }

    private void Move()
    {
        _direction = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Direction = -1f;
            _dashDirection = -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Direction = 1f;
            _dashDirection = 1f;
        }

        if (!CheckMovePath())
        {
            _player.Rb.velocity = new Vector2(0, _player.Rb.velocity.y);

            if (_direction == 0)
            {
                _player.Ani.SetFloat("Speed", 0);
            }
            return;
        }

        Vector3 tempVelocity = new Vector3();

        if (_player.isTurn)
        {
            tempVelocity = new Vector3(0, _player.Rb.velocity.y, _direction * _player.Stat.SprintSpeed);
            _player.Ani.SetFloat("Speed", Mathf.Abs(tempVelocity.z));
        }
        else
        {
            tempVelocity = new Vector2(_direction * _player.Stat.SprintSpeed, _player.Rb.velocity.y);
            _player.Ani.SetFloat("Speed", Mathf.Abs(tempVelocity.x));
        }

        _player.Rb.velocity = tempVelocity;
    }


    private void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && !_isGrounded && !_isDoubleJumping)
        {
            _player.Ani.SetTrigger("isJumping");
            _player.Rb.velocity = new Vector2(_player.Rb.velocity.x, _player.PlayerSt.JumpForce);
            _isDoubleJumping = true;
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && _isGrounded)
        {
            _player.Ani.SetTrigger("isJumping");
            _player.Rb.velocity = new Vector2(_player.Rb.velocity.x, _player.PlayerSt.JumpForce);
            _isGrounded = false;
        }
        else if ((Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space)))
        {
            _player.Rb.velocity = new Vector3(_player.Rb.velocity.x, _player.Rb.velocity.y / 2, _player.Rb.velocity.z);
        }

        if (Mathf.Abs(_player.Rb.velocity.y) >= 0.1f)
        {
            _player.Ani.SetFloat("VerticalSpeed", _player.Rb.velocity.y);
        }
    }

    private void Dash()
    {
        _player.Rb.velocity = Vector2.zero;

        _isDashing = true;
        _player.GetComponent<Collider>().enabled = false; 
        Vector3 dashPosition = Vector3.zero;

        RaycastHit hit;

        if (Physics.Raycast(_player.transform.position, Vector3.right * _dashDirection, out hit, _player.PlayerSt.DashDistance, _player.WallLayer) ||
            Physics.Raycast(_player.transform.position, Vector3.right * _dashDirection, out hit, _player.PlayerSt.DashDistance, _player.GroundLayer)) // 벽이 있다면 벽과의 충돌 위치 바로 앞에서 멈추게 설정
        {            
            dashPosition = hit.point - (Vector3.right * _dashDirection) * 0.2f;  // 곱하는 수 만큼 벽에서 떨어짐
        }
        else  // 벽이 없으면 대쉬 거리만큼 앞으로 이동
        {      
            dashPosition = _player.transform.position + (Vector3.right * _dashDirection) * _player.PlayerSt.DashDistance;
        }

        _player.Rb.DOMove(dashPosition, _player.PlayerSt.DashDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            _isDashing = false;
            _player.GetComponent<Collider>().enabled = true;

        });

        _player.Ani.SetTrigger("Dash");

        _dashTimer = 0;
    }

    private void Stop()
    {
        _player.Rb.velocity = new Vector2(0, _player.Rb.velocity.y);
    }

    private void Flip(float value)
    {
        Vector3 tempScale = _player.CharacterModel.localScale;

        if (value * tempScale.x < 0)
        {
            tempScale.x *= -1;
        }

        _player.CharacterModel.localScale = tempScale;
    }
    private bool CheckMovePath()
    {
        // 레이캐스트로 장애물 감지
        RaycastHit hit;
        if (Physics.Raycast(_player.transform.position, Vector2.right * _dashDirection, out hit, 0.5f, _player.BlockLayer))
        {
            // 장애물이 레이캐스트 범위 안에 있음
            //Debug.Log("장애물 감지: " + hit.collider.name);
            return false;
        }

        // 장애물이 없음
        return true;
    }

    public bool IsOnSlope()
    {
        RaycastHit slopeHit;
        Ray ray = new Ray(_player.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit, 3f, _player.GroundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //return angle != 0f && angle < 
        }

        return false;
    }

    private void RecordInput(KeyCode key)
    {
        if (Time.time - lastInputTime <= inputTimeLimit)
        {
            keyInputs.Add(key);
        }
        else
        {
            keyInputs.Clear();
            keyInputs.Add(key);
        }

        lastInputTime = Time.time;
    }

    private bool CheckDash()
    {
        if (keyInputs.Count == 2)
        {
            if (keyInputs[0] == keyInputs[1])
            {
                keyInputs.Clear();
                return true;
            }
        }

        return false;
    }
}
