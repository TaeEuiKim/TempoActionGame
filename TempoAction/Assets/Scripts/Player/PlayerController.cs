using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Linq;
using Cinemachine.Utility;
public class PlayerController
{
    private Player _player;

    [Header("키 입력 기록 리스트")]
    private List<KeyCode> keyInputs = new List<KeyCode>();
    [Header("키 입력 시간 제한")]
    private float inputTimeLimit = 0.2f;
    private float lastInputTime;

    private bool _isLanded;
    private bool _isOnMonster;
    private bool _isBackDash;

    public bool _isGrounded;
    public bool isUltimate;
    public bool isDashing;
    public bool isMove;
    public bool isJump;
    public bool isDown = false;

    private float _dashTimer;
    protected float _direction;
    protected float _dashDirection;

    private RaycastHit slopeHit;

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
        isDashing = false;
        isMove = true;
        isJump = true;

        _dashDirection = -_player.CharacterModel.localScale.x;
        _dashTimer = 0f;

        _dashTimer = _player.PlayerSt.DashDelay;
    }

    public void Update()
    {
        if (_player.PlayerSt.IsKnockedBack) return;

        InvokeUltimate();

        if (_dashTimer >= _player.PlayerSt.DashDelay)
        {
            if (PlayerInputManager.Instance.leftArrow || PlayerInputManager.Instance.rightArrow)
            {
                PlayerInputManager.Instance.leftArrow = false;
                PlayerInputManager.Instance.rightArrow = false;
            }

            if ((PlayerInputManager.Instance.sway || PlayerInputManager.Instance.dash || CheckDash()))
            {
                Dash();
            }
        }
        else
        {
            _dashTimer += Time.deltaTime;
        }

        if (!(_isGrounded = Physics.CheckSphere(_player.GroundCheckPoint.position, _player.GroundCheckRadius, _player.GroundLayer)))
        {
            _isGrounded = Physics.CheckSphere(_player.GroundCheckPoint.position, _player.GroundCheckRadius, _player.WallLayer);
        }
        _isOnMonster = Physics.CheckSphere(_player.GroundCheckPoint.position, _player.GroundCheckRadius, _player.BossLayer);
        _player.Ani.SetBool("isGrounded", _isGrounded);

        if (PlayerInputManager.Instance.downArrow)
        {
            _isBackDash = true;
        }
        else
        {
            if (!isDashing)
            {
                _isBackDash = false;
            }
        }

        if (_player.Attack.CurrentAttackkState == Define.AttackState.ATTACK)
        {
            Stop();

            return;
        }

        if (_isGrounded)
        {
            if (!_isLanded)
            {
                _isLanded = true;
            }
        }
        else
        {
            _isLanded = false;
        }

        if (_isOnMonster && !_isGrounded)
        {
            Vector3 force = new Vector3(-_player.CharacterModel.localScale.x * 10f, -5f);
            _player.Rb.AddForce(force, ForceMode.VelocityChange);
        }

        if (!isDashing)
        {
            Move();
            Jump();
        }

        if (PlayerInputManager.Instance.downArrow && _isGrounded)
        {
            isDown = true;
        }
    }

    private void Move()
    {
        Direction = 0f;

        if (PlayerInputManager.Instance.move.x < 0)
        {
            Direction = -1f;
            _dashDirection = -1f;
        }
        if (PlayerInputManager.Instance.move.x > 0)
        {
            Direction = 1f;
            _dashDirection = 1f;
        }

        if (!isMove)
        {
            return;
        }

        if (!CheckMovePath())
        {
            _player.Rb.velocity = new Vector2(0, _player.Rb.velocity.y);

            if (Direction == 0)
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
            tempVelocity = new Vector3(_direction * _player.Stat.SprintSpeed, _player.Rb.velocity.y);
            _player.Ani.SetFloat("Speed", Mathf.Abs(tempVelocity.x));
        }

        _player.Rb.velocity = tempVelocity;
    }

    public void Jump(bool useKeyDown = true)
    {
        if (!isJump || _player.Ani.GetBool("IsCommand"))
        {
            PlayerInputManager.Instance.jump = false;
            return;
        }    

        if(!useKeyDown || PlayerInputManager.Instance.jump)
        {
            PlayerInputManager.Instance.jump = false;

            if (_isGrounded)
            {
                _player.Ani.SetTrigger("isJumping");
                _player.Rb.velocity = new Vector3(_player.Rb.velocity.x, _player.PlayerSt.JumpForce);
                TestSound.Instance.PlaySound("Jump");
                _isGrounded = false;

                GameObject effect = ObjectPool.Instance.Spawn("FX_Jump", 1);

                if (effect == null) { return; }

                effect.transform.position = _player.transform.position + new Vector3(0, 0.2f);
            }
            else
            {
                //_player.Rb.velocity = new Vector3(_player.Rb.velocity.x, _player.Rb.velocity.y / 2, _player.Rb.velocity.z);
            }
        }
        

        if (Mathf.Abs(_player.Rb.velocity.y) >= 0.1f)
        {
            _player.Ani.SetFloat("VerticalSpeed", _player.Rb.velocity.y);
        }
    }

    private void Dash()
    {
        if (!_isGrounded)
        {
            return;
        }

        // 커맨드 초기화
        PlayerInputManager.Instance.ResetCommandKey();
        _player.Ani.SetBool("IsCommand", false);
        isJump = false;
        _player.IsInvincible = true;

        if (PlayerInputManager.Instance.move.x < 0)
        {
            _dashDirection = -1f;
        }
        if (PlayerInputManager.Instance.move.x > 0)
        {
            _dashDirection = 1f;
        }

        //isDashing = true;
        _player.transform.DOKill();
        _player.Ani.SetFloat("Speed", 0);
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.FINISH);

        CoroutineRunner.Instance.StartCoroutine(DashInvincibility(_player.PlayerSt.DashDuration));
        //_player.Rb.velocity = Vector2.zero;
        Vector3 dashPosition = Vector3.zero;
        RaycastHit hit;

        float dir = 1;

        if (PlayerInputManager.Instance.sway)
        {
            PlayerInputManager.Instance.sway = false;
            dir = -1;
            _isBackDash = true;
        }

        if (_isBackDash)
        {
            dir = -1;
            TestSound.Instance.PlaySound("Backdash");
        }
        else
        {
            dir = 1;
            TestSound.Instance.PlaySound("Frontdash");
        }
        _player.Ani.SetBool("IsBackDash", _isBackDash);

        _player.CharacterModel.localScale = new Vector3(-_dashDirection, 1, -1);

        if (Physics.Raycast(_player.transform.position + new Vector3(0, 0.5f), Vector3.right * _dashDirection * dir, out hit, _player.PlayerSt.DashDistance, _player.WallLayer) ||
            Physics.Raycast(_player.transform.position + new Vector3(0, 0.5f), Vector3.right * _dashDirection * dir, out hit, _player.PlayerSt.DashDistance, _player.GroundLayer))
        {
            dashPosition = (hit.point - new Vector3(0, 0.5f)) - (Vector3.right * _dashDirection * dir) * 0.2f;  // 곱하는 수 만큼 벽에서 떨어짐
        }
        else  // 벽이 없으면 대쉬 거리만큼 앞으로 이동
        {      
            dashPosition = _player.transform.position + (Vector3.right * _dashDirection * dir) * _player.PlayerSt.DashDistance;
        }

        _player.Rb.DOMove(dashPosition, _player.PlayerSt.DashDuration).OnComplete(() => { isJump = true; isMove = true; _player.IsInvincible = false; });

        _player.Ani.SetTrigger("Dash");

        _dashTimer = 0;
    }

    private IEnumerator DashInvincibility(float duration)
    {
        float invincibilityTime = duration / 2;

        yield return new WaitForSeconds(duration / 4);

        _player.IsInvincible = true;

        yield return new WaitForSeconds(invincibilityTime);

        _player.IsInvincible = false;
    }

    private void Stop()
    {
        _player.Rb.velocity = new Vector2(0, _player.Rb.velocity.y);
    }

    private void InvokeUltimate()
    {
        if (PlayerInputManager.Instance.ultimate && !isUltimate && _player.View.GetUltimateGauge() == 1)
        {
            PlayerInputManager.Instance.ultimate = false;
            isUltimate = true;

            GameObject disappearEffect = ObjectPool.Instance.Spawn("do_disappear", 1f);
            disappearEffect.transform.position = _player.SkillObject.transform.position;
            _player.SkillObject.SetActive(false);

            TestSound.Instance.PlaySound("UltimateOn");
            TestSound.Instance.PlaySound("Skill1");

            _player.RimShader.SetFloat("_Float", 1f);
            for (int i = 0; i < _player.MoveEffect.Length; ++i)
            {
                _player.MoveEffect[i].SetActive(true);
            }

            _player.CharacterModel.GetComponent<CharacterTrail>().StartTrail(10f);

            _player.PowerUp(1.25f);
            _player.View.UiEffect.SetActive(false);
            _player.View.UseUltimate();
        }
        else if (PlayerInputManager.Instance.ultimate && isUltimate)
        {
            PlayerInputManager.Instance.ultimate = false;
        }
    }

    private void Flip(float value)
    {
        Vector3 tempScale = _player.CharacterModel.localScale;

        if (value * tempScale.x > 0)
        {
            tempScale.x *= -1;
        }

        _player.CharacterModel.localScale = tempScale;
    }

    private bool CheckMovePath()
    {
        // 레이캐스트로 장애물 감지
        RaycastHit hit;
        if (Physics.Raycast(_player.transform.position, Vector2.right * _dashDirection, out hit, 0.5f, _player.WallLayer))
        {
            // 장애물이 레이캐스트 범위 안에 있음
            //Debug.Log("장애물 감지: " + hit.collider.name);
            return false;
        }

        // 장애물이 없음
        return true;
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

    public void SetBackDash()
    {
        _isBackDash = false;
        PlayerInputManager.Instance.downArrow = false;
    }
}
