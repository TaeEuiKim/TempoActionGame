using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{

    private Player _player;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private bool _isLanded = false;
    private bool _isGrounded = true;
    private bool _isDashing = false;
    private bool _facingRight = true;
    //private bool _isMoved = false;

    private float _moveInput = 0;
    private float _dashTimer = 0f;
    public float Direction 
    {
        get
        {
            return _facingRight ? 1 : -1; ;
        }
    }

    private void Start()
    {
        _player = transform.parent.GetComponent<Player>();

        _dashTimer = _player.Stat.DashDelay;
    }

    public void Stay()
    {

        if (_player.Stat.IsKnockedBack) return;

        if (_player.CurAtkState == Define.AtkState.ATTACK)
        {
            _player.Rb.velocity = new Vector2(0, _player.Rb.velocity.y);

            return;
        }

        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
        _player.Ani.SetBool("isGrounded", _isGrounded);

        if (_isGrounded)
        {
            if (!_isLanded)
            {
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_JumpLanding", transform);
                _isLanded = true;
            }
        }
        else
        {
            _isLanded = false;
        }

        if (_dashTimer >= _player.Stat.DashDelay)
        {
            if (Input.GetKeyDown(KeyCode.D))
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
    }

    private void Move()
    {
        _moveInput = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _moveInput = -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _moveInput = 1f;
        }


        Vector2 tempVelocity = new Vector2(_moveInput * _player.Stat.SprintSpeed, _player.Rb.velocity.y);

        _player.Ani.SetFloat("Speed", Mathf.Abs(tempVelocity.x));


        _player.Rb.velocity = tempVelocity;

        if (_moveInput > 0 && !_facingRight)
        {
            Flip();
        }
        else if (_moveInput < 0 && _facingRight)
        {
            Flip();
        }
    }


    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && _isGrounded)
        {
            _player.Ani.SetTrigger("isJumping");
            _player.Rb.velocity = new Vector2(_player.Rb.velocity.x, _player.Stat.JumpForce);
            _isGrounded = false;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
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
        //if (!_isGrounded) return;

        _isDashing = true;

        Vector2 dashPosition = (Vector2)_player.Rb.position + Vector2.right * Direction * _player.Stat.DashDistance;

        _player.Rb.DOMove(dashPosition, _player.Stat.DashDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            _isDashing = false;
        });

        // Trigger dash animation
        _player.Ani.SetTrigger("Dash");

        _dashTimer = 0;
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        float scaleX = _facingRight ? 1 : -1;
        transform.localScale = new Vector3(scaleX, 1, 1);
    }

    private void PlayerSfx(Define.PlayerSfxType type)
    {
        switch (type)
        {
            case Define.PlayerSfxType.MAIN:
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_RhythmCombo_Attack_" + (_player.Atk.AttackIndex + 1), transform);
                break;
            case Define.PlayerSfxType.POINT:
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_PointTempo_Hit", transform);
                break;
            case Define.PlayerSfxType.DASH:
                break;
            case Define.PlayerSfxType.JUMP:
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_Jump", transform);
                break;
            case Define.PlayerSfxType.RUN:
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_Running", transform);
                break;
            case Define.PlayerSfxType.STUN:
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_Overload_Occurred", transform);
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_Overload_Recovery", transform);
                break;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }

}
