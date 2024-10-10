using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private Player _player;
    private CameraController _cameraController;

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

        _player.Attack.IsHit = true;

        foreach (Collider monsterCollider in hitMonsters)
        {
            Monster monster = monsterCollider.GetComponent<Monster>();

            if (_player.Attack.CurrentTempoData.type == Define.TempoType.MAIN)
            {
                HitMainTempo(monster);
            }
            else
            {
                HitPointTempo(monster);
            }
            // ��Ʈ ��ƼŬ ����
            GameObject hitParticle = SpawnHitParticle(monster, "P_Punch");
            GameObject hitParticle2 = SpawnHitParticle(monster, "P_PunchAttack");

            Vector3 hitPos = monsterCollider.ClosestPoint(_player.HitPoint.position);
            hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z - 0.1f);
            hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(-_player.CharacterModel.localScale.x, 0, 0));
            hitParticle2.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z);
        }
    }
    private void HitMainTempo(Monster monster)
    {
        CameraShaking(0.2f);
        // ���� ������ �� ������ ó��
        monster.TakeDamage(_player.GetTotalDamage());
    }
    private void HitPointTempo(Monster monster)
    {
        //Define.CircleState state = _player.Attack.PointTempoCircle.CircleState;
        float damage = _player.GetTotalDamage();
        monster.TakeDamage(damage);
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

    private void StartPointTempo()
    {
        _player.IsInvincible = true;
    }

    // ����Ʈ ���� �ִϸ��̼� ���� �߰��ϴ� �̺�Ʈ �Լ�
    private void FinishPointTempo()
    {
        if (_player.Attack.IsHit)
        {
        }

        _player.Attack.IsHit = false;

        _player.IsInvincible = false;
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.FINISH);
    }

    // ���� ���� �ִϸ��̼� ���� �߰��ϴ� �̺�Ʈ �Լ�
    private void Finish(float delay)
    {
        _player.Attack.IsHit = false;
        _player.Attack.CheckDelay = delay;
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.CHECK);
    }

    // ���� ��Ÿ� �ȿ� ���� ������ �� ������ �̵��ϴ� �̺�Ʈ �Լ�
    private void MoveToClosestMonster(float duration)
    {
        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Vector3 rayDirection = transform.localScale.x > 0 ? transform.right : transform.right * -1;

        // ����ĳ��Ʈ ����
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, _player.Attack.CurrentTempoData.distance, _player.MonsterLayer))
        {
            float closestMonsterX = hit.point.x + (-rayDirection.x * 0.2f);
            transform.parent.DOMoveX(closestMonsterX, duration);
        }


        // ����׿� ���� �׸���
        Debug.DrawRay(rayOrigin, rayDirection * _player.Attack.CurrentTempoData.distance, Color.red);
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
