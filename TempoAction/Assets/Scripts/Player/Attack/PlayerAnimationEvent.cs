using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Unity.VisualScripting;
using System.Threading;
using UnityEngine.UIElements;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Transform leftHandTrans;
    [SerializeField] private Transform rightHandTrans;
    [SerializeField] private Transform leftFootTrans;
    [SerializeField] private Transform rightFootTrans;
    [SerializeField] private Transform rightHandTrail;

    private CameraController _cameraController;

    [SerializeField] Vector3 tempVec;

    private void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();
    }

    #region Hit
    // �÷��̾� Ÿ�� ��ġ�� ����ϴ� �̺�Ʈ �Լ�
    private void Hit()
    {
        Collider[] hitMonsters = Physics.OverlapBox(_player.HitPoint.position, _player.ColliderSize / 2, _player.HitPoint.rotation, _player.MonsterLayer);

        if (hitMonsters.Length <= 0) return;

        foreach (Collider monsterCollider in hitMonsters)
        {
            Monster monster = monsterCollider.GetComponent<Monster>();
            if (_player.Ani.GetBool("IsCommand"))
            {
                GameObject hitParticle;
                GameObject hitParticle2;
                GameObject hitParticle3;

                switch (_player.Ani.GetInteger("CommandCount"))
                {
                    case 1:
                        hitParticle = ObjectPool.Instance.Spawn("P_SmashAttack_01", 1);
                        hitParticle2 = ObjectPool.Instance.Spawn("P_SmashAttack_02", 1);
                        hitParticle.transform.position = leftHandTrans.position + new Vector3(-0.3f * _player.CharacterModel.localScale.x, 0);
                        hitParticle2.transform.position = leftHandTrans.position + new Vector3(-0.3f * _player.CharacterModel.localScale.x, 0);
                        if (_player.CharacterModel.localScale.x < 0)
                        {
                            hitParticle.transform.rotation = leftHandTrans.rotation * Quaternion.Euler(tempVec);
                            hitParticle2.transform.rotation = leftHandTrans.rotation * Quaternion.Euler(tempVec);
                        }
                        else
                        {
                            hitParticle.transform.rotation = leftHandTrans.rotation;
                            hitParticle2.transform.rotation = leftHandTrans.rotation;
                        }
                        break;
                    case 2:
                        rightHandTrail.gameObject.SetActive(true);
                        hitParticle = ObjectPool.Instance.Spawn("P_SmashAttack2", 1);
                        hitParticle2 = ObjectPool.Instance.Spawn("P_SmashHit", 1);
                        hitParticle3 = ObjectPool.Instance.Spawn("P_dust", 1);

                        hitParticle.transform.position = rightHandTrans.position + new Vector3(-0.7f * _player.CharacterModel.localScale.x, 0);
                        hitParticle2.transform.position = rightHandTrans.position + new Vector3(-0.7f * _player.CharacterModel.localScale.x, 0);
                        hitParticle3.transform.position = leftFootTrans.position + new Vector3(-0.4f * _player.CharacterModel.localScale.x, -0.2f);

                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle3.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));

                        //hitParticle.transform.rotation = rightHandTrans.rotation;
                        break;
                    default:
                        hitParticle = ObjectPool.Instance.Spawn("FX_PunchAttackSphere", 1);

                        hitParticle.transform.position = monsterCollider.ClosestPoint(_player.HitPoint.position);
                        break;
                }
            }
            else
            {
                // ��Ʈ ��ƼŬ ����
                GameObject hitParticle = ObjectPool.Instance.Spawn("P_Punch", 1);
                GameObject hitParticle2 = ObjectPool.Instance.Spawn("P_PunchAttack", 1);

                switch (_player.Ani.GetInteger("AtkCount"))
                {
                    case 0:
                        hitParticle.transform.position = leftHandTrans.position;
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle2.transform.position = leftHandTrans.position;
                        hitParticle2.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle.transform.rotation = leftHandTrans.rotation;
                        hitParticle2.transform.rotation = leftHandTrans.rotation;
                        break;
                    case 1:
                        hitParticle.transform.position = rightHandTrans.position;
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle2.transform.position = rightHandTrans.position;
                        hitParticle2.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle.transform.rotation = rightHandTrans.rotation;
                        hitParticle2.transform.rotation = rightHandTrans.rotation;
                        break;
                    case 2:
                        hitParticle.transform.position = leftHandTrans.position;
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle2.transform.position = leftHandTrans.position;
                        hitParticle2.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle.transform.rotation = leftHandTrans.rotation;
                        hitParticle2.transform.rotation = leftHandTrans.rotation;
                        break;
                    case 3:
                        hitParticle.transform.position = leftHandTrans.position;
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle2.transform.position = leftHandTrans.position;
                        hitParticle2.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle.transform.rotation = leftHandTrans.rotation;
                        hitParticle2.transform.rotation = leftHandTrans.rotation;
                        break;
                }
            }

            HitMainTempo(monster);
        }
    }
    private void HitMainTempo(Monster monster)
    {
        CameraShaking(0.2f);
        // ���� ������ �� ������ ó��
        monster.TakeDamage(_player.GetTotalDamage());
    }

    private GameObject SpawnHitParticle(Monster monster, string name)
    {
        GameObject hitParticle = null;

        if (monster.IsGuarded)
        {
            float direction = transform.position.x - monster.transform.position.x;

            direction = direction > 0 ? 1 : -1;

            // ���� �浹 ����Ʈ ����
            hitParticle = ObjectPool.Instance.Spawn("FX_EliteHitGuard", 1);
            hitParticle.transform.localScale = new Vector3(direction, 1, 1);
        }
        else
        {
            if (_player.Attack.CurrentTempoData.type == Define.TempoType.POINT)
            {
                hitParticle = ObjectPool.Instance.Spawn(name, 1);
            }
            else
            {
                hitParticle = ObjectPool.Instance.Spawn(name, 1);
            }
        }

        return hitParticle;
    }

    private void CameraShaking(float shakeTime)
    {
        if (!_cameraController)
        {
            Debug.LogWarning("ī�޶� ��Ʈ�ѷ� ����");
        }

        _cameraController.VibrateForTime(shakeTime);
    }

    #endregion

    // ���� ���� �ִϸ��̼� ���� �߰��ϴ� �̺�Ʈ �Լ�
    private void Finish(float delay)
    {
        _player.Attack.CheckDelay = delay;
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.CHECK);
        _player.Ani.SetBool("IsCommand", false);
        _player.Ani.SetInteger("CommandCount", 0);
        PlayerInputManager.Instance.isCommand = false;
    }

    private void RemoveTrail()
    {
        if (rightHandTrail.gameObject.activeSelf)
        {
            rightHandTrail.gameObject.SetActive(false);
        }
    }

    private void JumpRock()
    {
        if (_player.Ani.GetFloat("VerticalSpeed") <= -15)
        {
            _player.Rb.isKinematic = true;
        }
    }

    private void JumpFinish()
    {
        if (_player.Rb.isKinematic)
        {
            _player.Rb.isKinematic = false;
            _player.Ani.SetFloat("VerticalSpeed", 0);
        }

        _player.Attack.ChangeCurrentAttackState(Define.AttackState.FINISH);
    }

    private void JumpFalling()
    {
        _player.Rb.AddForce(Vector3.down * 50f, ForceMode.Impulse);
    }

    private void FinishDash()
    {
        _player.Ani.SetBool("IsBackDash", false);
        _player.Controller.isDashing = false;
        _player.Controller.isMove = true;
    }

    // ���� ��Ÿ� �ȿ� ���� ������ �� ������ �̵��ϴ� �̺�Ʈ �Լ�
    private void MoveToClosestMonster(float duration)
    {
        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Vector3 rayDirection = transform.localScale.x < 0 ? transform.right : transform.right * -1;

        // ����ĳ��Ʈ ����
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, _player.Attack.CurrentTempoData.distance, _player.MonsterLayer))
        {
            float closestMonsterX = hit.point.x + (-rayDirection.x * 0.2f);
            transform.parent.DOMoveX(closestMonsterX, duration);
        }
    }

    private void MoveAttack(float moveDistance)
    {
        _player.Ani.SetFloat("Speed", 0);

        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Vector3 rayDirection = transform.localScale.x < 0 ? transform.right : transform.right * -1;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitPos, _player.Attack.CurrentTempoData.distance, _player.BlockLayer))
        {
            float closestMonsterX = hitPos.point.x + (-rayDirection.x * 1f);
            transform.parent.DOMoveX(closestMonsterX, 0.3f);
        }
        else
        {
            if (_player.Ani.GetBool("IsCommand"))
            {
                switch (_player.Ani.GetInteger("CommandCount"))
                {
                    case 1:
                    case 2:
                        transform.parent.DOMoveX(transform.parent.position.x - (moveDistance * _player.CharacterModel.localScale.x), 0.3f);
                        break;
                    case 4:
                        Vector3 targetPos = new Vector3(transform.parent.position.x - (moveDistance * _player.CharacterModel.localScale.x)
                                                       , transform.parent.position.y + 4f, 0);
                        transform.parent.DOMove(targetPos, 0.7f);
                        break;
                }
            }
            else
            {
                switch (_player.Ani.GetInteger("AtkCount"))
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    default:
                        transform.parent.DOMoveX(transform.parent.position.x - (moveDistance * _player.CharacterModel.localScale.x), 0.3f);
                        break;
                    case 4:
                        transform.parent.DOMoveX(transform.parent.position.x - (moveDistance * _player.CharacterModel.localScale.x), 0.3f);
                        //_player.Rb.AddForce(Vector3.right * _player.Controller.Direction * moveDistance, ForceMode.VelocityChange);
                        break;
                }
            }
        }
    }

    private void CheckCommand(int SkillId)
    {
        AnimatorStateInfo stateInfo = _player.Ani.GetCurrentAnimatorStateInfo(0); // ���̾� 0���� ��� ���� �ִϸ��̼� ����

        // ���� �ִϸ��̼��� ���� �ð� ���
        float animationLength = stateInfo.length; // �ִϸ��̼� ��ü ����
        float currentPlayTime = stateInfo.normalizedTime * animationLength; // ���� ����� �ð� (normalizedTime�� 0���� 1 ������ ��)

        float timeRemaining = animationLength - currentPlayTime - 0.12f; // ���� �ð� ���

        _player.Ani.SetBool("IsCommand", true);

        _player.CommandController.StartCommandTime(timeRemaining, SkillId);
    }

    private void LeftFootEffect()
    {
        GameObject effect = ObjectPool.Instance.Spawn("FX_Walk", 1);
        effect.transform.position = leftFootTrans.position;
    }

    private void RightFootEffect()
    {
        GameObject effect = ObjectPool.Instance.Spawn("FX_Walk", 1);
        effect.transform.position = rightFootTrans.position;
    }

    // �ð� ũ�� ����
    private void ChangeTimeScale(float value)
    {
        Time.timeScale = value;
    }
    // �ð� ũ�� ����
    private void ReturnTimeScale()
    {
        Time.timeScale = 1;
    }

    //Ÿ�Ӷ��� ���� �Լ�
    private void StartTimeline(string name)
    {
        TimelineManager.Instance.PlayTimeline(name);
    }

    private void PlayerSfx(Define.PlayerSfxType type)
    {
        switch (type)
        {
            case Define.PlayerSfxType.MAIN:
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_RhythmCombo_Hit_" + (_player.Attack.CurrentTempoData.attackNumber + 1), transform);
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
}
