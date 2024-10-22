using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Linq;
public class PlayerController
{
    private Player _player;
    private SkillCommand _skillCommand;

    [Header("키 입력 기록 리스트")]
    private List<KeyCode> keyInputs = new List<KeyCode>();
    [Header("키 입력 시간 제한")]
    private float inputTimeLimit = 0.2f;
    private float lastInputTime;

    private bool _isLanded;
    private bool _isGrounded;
    private bool _isOnMonster;
    private bool _isDoubleJumping;
    public bool isDashing;
    public bool isMove;
    public bool isJump;
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
        _skillCommand = player._skillCommand;
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

        if (_dashTimer >= _player.PlayerSt.DashDelay)
        {
            if (PlayerInputManager.Instance.leftArrow)
            {
                PlayerInputManager.Instance.leftArrow = false;
                RecordInput(KeyCode.LeftArrow);
            }
            else if (PlayerInputManager.Instance.rightArrow)
            {
                PlayerInputManager.Instance.rightArrow = false;
                RecordInput(KeyCode.RightArrow);
            }

            if ((PlayerInputManager.Instance.dash || CheckDash()))
            {
                Dash();
            }
        }
        else
        {
            _dashTimer += Time.deltaTime;
        }

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

        if (_isOnMonster && !_isGrounded)
        {
            Vector3 force = new Vector3(-_player.CharacterModel.localScale.x * 10f, -5f);
            _player.Rb.AddForce(force, ForceMode.VelocityChange);
        }

        if (PlayerInputManager.Instance.downArrow)
        {
            _player.Ani.SetBool("IsBackDash", true);
        }
        else
        {
            _player.Ani.SetBool("IsBackDash", false);
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

        if (!isMove)
        {
            return;
        }

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
            tempVelocity = new Vector2(_direction * _player.Stat.SprintSpeed, _player.Rb.velocity.y);
            _player.Ani.SetFloat("Speed", Mathf.Abs(tempVelocity.x));
        }

        _player.Rb.velocity = tempVelocity;
    }


    public void Jump(bool useKeyDown = true)
    {
        if (!isJump)
        {
            return;
        }    

        if(!useKeyDown || PlayerInputManager.Instance.jump)
        {
            PlayerInputManager.Instance.jump = false;
            if (!_isGrounded && !_isDoubleJumping)
            {
                _player.Ani.SetTrigger("isJumping");
                _player.Rb.velocity = new Vector2(_player.Rb.velocity.x, _player.PlayerSt.JumpForce);
                _isDoubleJumping = true;
            }
            if (_isGrounded)
            {
                _player.Ani.SetTrigger("isJumping");
                _player.Rb.velocity = new Vector2(_player.Rb.velocity.x, _player.PlayerSt.JumpForce);
                _isGrounded = false;

                GameObject effect = ObjectPool.Instance.Spawn("FX_Jump", 1);
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

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _dashDirection = -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _dashDirection = 1f;
        }

        isDashing = true;
        _player.transform.DOKill();
        _player.Ani.SetFloat("Speed", 0);
        _player.Rb.velocity = Vector3.zero;
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.FINISH);

        CoroutineRunner.Instance.StartCoroutine(DashInvincibility(_player.PlayerSt.DashDuration));
        //_player.Rb.velocity = Vector2.zero;
        Vector3 dashPosition = Vector3.zero;
        RaycastHit hit;

        float dir = 1;
        if (_player.Ani.GetBool("IsBackDash"))
        {
            dir = -1;
        }

        _player.CharacterModel.localScale = new Vector3(-_dashDirection, 1, -1);

        if (Physics.Raycast(_player.transform.position + new Vector3(0, 0.5f), Vector3.right * _dashDirection * dir, out hit, _player.PlayerSt.DashDistance, _player.WallLayer) ||
            Physics.Raycast(_player.transform.position + new Vector3(0, 0.5f), Vector3.right * _dashDirection * dir, out hit, _player.PlayerSt.DashDistance, _player.GroundLayer))
        {
            dashPosition = (hit.point - new Vector3(0, 0.5f)) - (Vector3.right * _dashDirection * dir) * 0.2f;  // 곱하는 수 만큼 벽에서 떨어짐
            isMove = true;
        }
        else  // 벽이 없으면 대쉬 거리만큼 앞으로 이동
        {      
            dashPosition = _player.transform.position + (Vector3.right * _dashDirection * dir) * _player.PlayerSt.DashDistance;
        }

        _player.Rb.DOMove(dashPosition, _player.PlayerSt.DashDuration);

        _player.Ani.SetTrigger("Dash");

        _dashTimer = 0;
    }

    private IEnumerator DashInvincibility(float duration)
    {
        float invincibilityTime = duration / 2;

        yield return new WaitForSeconds(duration / 4);

        _player.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(invincibilityTime);

        _player.GetComponent<Collider>().enabled = true;
    }

    public void OnCommandTime(float checkTime, int attackCount)
    {
        isJump = false;
        isMove = false;

        PlayerInputManager.Instance.isCommand = true;
        CoroutineRunner.Instance.StartCoroutine(CheckCommandTime(checkTime, attackCount));
    }

    private IEnumerator CheckCommandTime(float checkTime, int attackCount)
    {
        float checkTimer = 0f;
        while (checkTimer < checkTime)
        {
            checkTimer += Time.deltaTime;
            yield return null;
        }

        CheckAttackCommand(PlayerInputManager.Instance.GetCommandKey(), attackCount);
        yield return null;
    }

    private void Stop()
    {
        _player.Rb.velocity = new Vector2(0, _player.Rb.velocity.y);
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

    private void CheckAttackCommand(List<KeyCode> keyList, int attackCount)
    {
        KeyCode[] keys;

        for (int i = 0; i < keyList.Count; ++i)
        {
            Debug.LogError(i + " : " + keyList[i]);
        }

        // AttackCount와 일치하는 스킬 아이디가 있는지 검사
        var commandData = _skillCommand.commandDatas
            .FirstOrDefault(data => data.SkillId == attackCount);

        if (commandData != null && commandData.KeyCodes.Length == keyList.Count)
        {
            keys = commandData.KeyCodes;
        }
        else
        {
            PlayerInputManager.Instance.isCommand = false;
            isMove = true;
            isJump = true;
            return;
        }

        // 키가 다 같으면
        if (keyList.Zip(keys, (key1, key2) => key1 == key2).All(isEqual => isEqual))
        {
            _player.Ani.SetInteger("CommandCount", attackCount);
            _player.Ani.SetBool("IsCommand", true);
            _player.SkillManager.SkillSlots[0].UseSkillInstant(_player);
            _player.Attack.ChangeCurrentAttackState(Define.AttackState.ATTACK);
        }

        PlayerInputManager.Instance.isCommand = false;
        isMove = true;
        isJump = true;
    }
}
