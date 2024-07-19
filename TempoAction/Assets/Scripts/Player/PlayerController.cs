using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{

    private PlayerManager _player;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private bool _isGrounded = true;
    private bool _isDashing = false;
    private bool _facingRight = true;

    public float DashDirection { get; set; } = 1;

    private void Start()
    {
        _player = transform.parent.GetComponent<PlayerManager>();
    }

    public void Stay()
    {
        if (_player.CurAtkState == Define.AtkState.ATTACK)
        {
            _player.Rb.velocity = new Vector2(0, _player.Rb.velocity.y);
            _player.Ani.SetFloat("Speed", 0);
            _player.Ani.SetFloat("VerticalSpeed", 0);
            return;
        }

        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
        _player.Ani.SetBool("isGrounded", _isGrounded);

        if (!_isDashing)
        {
            Move();
            Jump();
        }

        DashDirection = _facingRight ? 1 : -1;

        if (InputManager.Instance.GetInpuState("Dash") == Define.InputState.DOWN)
        {
            Dash();
        }

        if (_isGrounded)
        {
            if (_player.Atk.Index != 4)
            {
                if (InputManager.Instance.GetInpuState("MainTempo") == Define.InputState.DOWN)
                {
                    _player.Atk.Execute();
                }
               
            }
            else
            {
                if (_player.Atk.CircleState != Define.CircleState.NONE)
                {
                    if (_player.Atk.CircleState == Define.CircleState.BAD) // 포인트 템포 실패
                    {
                        _player.CurAtkState = Define.AtkState.FINISH;
                    }
                    else
                    {
                        _player.Atk.Execute();
                        _player.Atk.UpgradeCount++;
                    }
                }
            }

        }
        // Update animator parameters
        _player.Ani.SetFloat("Speed", Mathf.Abs(_player.Rb.velocity.x));

        if (Mathf.Abs(_player.Rb.velocity.y) >= 0.1f)
        {
            _player.Ani.SetFloat("VerticalSpeed", _player.Rb.velocity.y);
        }

    }

    private void Move()
    {
        float moveInput = 0;

        if (InputManager.Instance.GetInpuState("Left") == Define.InputState.STAY)
        {
            moveInput = -1f;
        }
        else if (InputManager.Instance.GetInpuState("Right") == Define.InputState.STAY)
        {
            moveInput = 1f;
        }

        _player.Rb.velocity = new Vector2(moveInput * _player.Stat.Speed, _player.Rb.velocity.y);

        if (moveInput > 0 && !_facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && _facingRight)
        {
            Flip();
        }
    }

    private void Jump()
    {
        if (InputManager.Instance.GetInpuState("Jump") == Define.InputState.DOWN && _isGrounded)
        {
            _player.Rb.velocity = new Vector2(_player.Rb.velocity.x, _player.Stat.JumpForce);
            _isGrounded = false;
        }
        else if (InputManager.Instance.GetInpuState("Jump") == Define.InputState.UP)
        {

            _player.Rb.velocity = new Vector3(_player.Rb.velocity.x, _player.Rb.velocity.y / 2, _player.Rb.velocity.z);

        }
    }

    private void Dash()
    {
        if (!_isGrounded) return;

        _isDashing = true;

        Vector2 dashPosition = (Vector2)_player.Rb.position + Vector2.right * DashDirection * _player.Stat.DashDistance;

        _player.Rb.DOMove(dashPosition, _player.Stat.DashDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            _isDashing = false;
        });

        // Trigger dash animation
        _player.Ani.SetTrigger("Dash");
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        float rotationY = _facingRight ? 0 : 180;
        transform.DOLocalRotate(new Vector3(0, rotationY, 0), 0.2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}
