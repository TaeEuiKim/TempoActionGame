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
    private CameraController _cameraController;

    private void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();
    }

    #region Hit
    // 플레이어 타격 위치에 사용하는 이벤트 함수
    private void Hit()
    {
        Collider[] hitMonsters = Physics.OverlapBox(_player.HitPoint.position, _player.ColliderSize / 2, _player.HitPoint.rotation, _player.MonsterLayer);

        if (hitMonsters.Length <= 0) return;

        _player.Attack.IsHit = true;

        // 히트 파티클 생성
        GameObject hitParticle = ObjectPool.Instance.Spawn("P_Punch", 1);
        GameObject hitParticle2 = ObjectPool.Instance.Spawn("P_PunchAttack", 1);

        foreach (Collider monsterCollider in hitMonsters)
        {
            Monster monster = monsterCollider.GetComponent<Monster>();

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
                case 4:
                    GameObject hitParticle3 = ObjectPool.Instance.Spawn("FX_PunchAttackSphere", 1);

                    hitParticle3.transform.position = monsterCollider.ClosestPoint(_player.HitPoint.position);
                    break;
            }

            HitMainTempo(monster);
        }
    }
    private void HitMainTempo(Monster monster)
    {
        CameraShaking(0.2f);
        // 메인 템포일 때 데미지 처리
        monster.TakeDamage(_player.GetTotalDamage());
    }

    private GameObject SpawnHitParticle(Monster monster, string name)
    {
        GameObject hitParticle = null;

        if (monster.IsGuarded)
        {
            float direction = transform.position.x - monster.transform.position.x;

            direction = direction > 0 ? 1 : -1;

            // 가드 충돌 이펙트 생성
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
            Debug.LogWarning("카메라 컨트롤러 없음");
        }

        _cameraController.VibrateForTime(shakeTime);
    }

    #endregion

    // 메인 템포 애니메이션 끝에 추가하는 이벤트 함수
    private void Finish(float delay)
    {
        _player.Attack.IsHit = false;
        _player.Controller.isMove = true;
        _player.Attack.CheckDelay = delay;
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.CHECK);
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
    }

    private void JumpFalling()
    {
        _player.Rb.AddForce(Vector3.down * 50f, ForceMode.Impulse);
    }

    private void StartDash()
    {
        _player.Ani.SetFloat("Speed", 0);
    }

    private void FinishDash()
    {
        _player.Ani.SetBool("IsBackDash", false);
        _player.Controller.isMove = true;
    }

    // 공격 사거리 안에 적이 있으면 적 앞으로 이동하는 이벤트 함수
    private void MoveToClosestMonster(float duration)
    {
        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Vector3 rayDirection = transform.localScale.x < 0 ? transform.right : transform.right * -1;

        // 레이캐스트 실행
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, _player.Attack.CurrentTempoData.distance, _player.MonsterLayer))
        {
            float closestMonsterX = hit.point.x + (-rayDirection.x * 0.2f);
            transform.parent.DOMoveX(closestMonsterX, duration);
        }

        // 디버그용 레이 그리기
        Debug.DrawRay(rayOrigin, rayDirection * _player.Attack.CurrentTempoData.distance, Color.red);
    }

    private void MoveAttack(float moveDistance)
    {
        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Vector3 rayDirection = transform.localScale.x < 0 ? transform.right : transform.right * -1;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitPos, _player.Attack.CurrentTempoData.distance, _player.MonsterLayer | _player.WallLayer))
        {
            float closestMonsterX = hitPos.point.x + (-rayDirection.x * 0.2f);
            transform.parent.DOMoveX(closestMonsterX, 0.3f);
        }
        else
        {
            switch (_player.Ani.GetInteger("AtkCount"))
            {
                case 0:
                    transform.parent.DOMoveX(transform.parent.position.x - (moveDistance * _player.CharacterModel.localScale.x), 0.3f);
                    break;
                case 1:
                    transform.parent.DOMoveX(transform.parent.position.x - (moveDistance * _player.CharacterModel.localScale.x), 0.3f);
                    break;
                case 2:
                    transform.parent.DOMoveX(transform.parent.position.x - (moveDistance * _player.CharacterModel.localScale.x), 0.3f);
                    break;
                case 3:
                    transform.parent.DOMoveX(transform.parent.position.x - (moveDistance * _player.CharacterModel.localScale.x), 0.3f);
                    break;
                case 4:
                    transform.parent.DOMoveX(transform.parent.position.x - (moveDistance * _player.CharacterModel.localScale.x), 0.3f);
                    //_player.Rb.AddForce(Vector3.right * _player.Controller.Direction * moveDistance, ForceMode.VelocityChange);
                    break;
            }
        }
    }

    private void CheckCommand()
    {
        _player.Controller.OnCommandTime(0.3f);
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

    // 시간 크기 변경
    private void ChangeTimeScale(float value)
    {
        Time.timeScale = value;
    }
    // 시간 크기 복구
    private void ReturnTimeScale()
    {
        Time.timeScale = 1;
    }

    //타임라인 실행 함수
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
