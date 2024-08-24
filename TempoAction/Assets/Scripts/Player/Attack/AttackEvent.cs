using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class AttackEvent : MonoBehaviour
{
    [SerializeField] private Player _player;


    // 플레이어 타격 위치에 사용하는 이벤트 함수
    private void Hit()
    {
        Collider[] hitMonsters = Physics.OverlapBox(_player.HitPoint.position, _player.ColliderSize / 2, _player.HitPoint.rotation, _player.MonsterLayer);

        if (hitMonsters.Length <= 0)
        {
            return;
        }

        if (_player.Attack.IsHit == false && _player.Attack.CurrentAttackTempoData.type == Define.TempoType.MAIN)
        {
            SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_RhythmCombo_Hit_" + (_player.Attack.AttackIndex + 1), transform);
        }
        _player.Attack.IsHit = true;


        foreach (Collider monster in hitMonsters)
        {

            Monster m = monster.GetComponent<Monster>();


            // 데미지 입히기
            if (_player.Attack.CurrentAttackTempoData.type == Define.TempoType.POINT)
            {
                if (_player.Attack.PointTempoCircle.CircleState == Define.CircleState.GOOD) // 타이밍이 Good일 경우
                {
                    m.TakeDamage(_player.GetTotalDamage(false), true);
                }
                else if (_player.Attack.PointTempoCircle.CircleState == Define.CircleState.PERFECT)
                {

                    m.TakeDamage(_player.GetTotalDamage(), true);
                }
            }
            else
            {
                m.TakeDamage(_player.GetTotalDamage());
            }

            // 히트 파티클 생성
            GameObject hitParticle = null;
            if (m.IsGuarded)
            {
                float direction = transform.position.x - m.transform.position.x;

                direction = direction > 0 ? 1 : -1;

                // 가드 충돌 이펙트 생성
                hitParticle = ObjectPool.Instance.Spawn("FX_EliteHitGuard", 1);
                hitParticle.transform.localScale = new Vector3(direction, 1,1);
            }
            else
            {
                if (_player.Attack.CurrentAttackTempoData.type == Define.TempoType.POINT)
                {
                    hitParticle = ObjectPool.Instance.Spawn("P_point_attack", 1);
                }
                else
                {
                    hitParticle = ObjectPool.Instance.Spawn("P_main_attack", 1);
                }
            }

            Vector3 hitPos = monster.ClosestPoint(_player.HitPoint.position);
            hitParticle.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z- 0.1f);

            _player.Attack.HitMonsterList.Add(monster.GetComponent<Monster>());
        }

    }


  

    // 포인트 템포 애니메이션 끝에 추가하는 이벤트 함수
    private void FinishPointTempo()
    {
        float addStamina = 0;

        if (_player.Attack.CurrentAttackTempoData.type == Define.TempoType.POINT) // 포인트 템포일 때
        {
            if (_player.Attack.IsHit)
            {
                addStamina = _player.Attack.CurrentAttackTempoData.maxStamina;

                if (_player.Attack.PointTempoCircle.CircleState == Define.CircleState.GOOD) // 타이밍이 Good일 경우
                {
                    addStamina = _player.Attack.CurrentAttackTempoData.minStamina;
                }

                _player.Attack.UpgradeCount++;

            }

        }

        _player.Stat.Stamina += addStamina;
        _player.UpdateStamina();

        _player.Attack.IsHit = false;

        _player.Attack.PointTempoCircle = null;
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.FINISH);
    }

    // 메인 템포 애니메이션 끝에 추가하는 이벤트 함수
    private void Finish(float delay)
    {

        float addStamina = 0;

        if (_player.Attack.IsHit)
        {
            addStamina = _player.Attack.CurrentAttackTempoData.maxDamage;
        }


        _player.Stat.Stamina += addStamina;
        _player.UpdateStamina();

        _player.Attack.IsHit = false;

        _player.Attack.CheckDelay = delay;
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.CHECK);


    }

    // 공격 사거리 안에 적이 있으면 적 앞으로 이동하는 이벤트 함수
    private void MoveToClosestMonster(float duration)
    {
        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Vector3 rayDirection = transform.localScale.x > 0 ? transform.right : transform.right * -1;

        // 레이캐스트 히트 정보 저장
        RaycastHit hit;

        // 레이캐스트 실행
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, _player.Attack.CurrentAttackTempoData.distance, _player.MonsterLayer))
        {
            float closestMonsterX = hit.point.x + (-rayDirection.x * 0.2f);
            transform.parent.DOMoveX(closestMonsterX, duration);
        }


        // 디버그용 레이 그리기
        Debug.DrawRay(rayOrigin, rayDirection * _player.Attack.CurrentAttackTempoData.distance, Color.red);
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

    // 중력 조절
    private void GravityActive(int value)
    {
        foreach (Monster monster in _player.Attack.HitMonsterList)
        {
            monster.Rb.useGravity = (value == 0) ? false : true;
        }
    }

    // 콜라이더 활성화/비활성화
    private void SetColliderActive(int value)
    {
        _player.GetComponent<Collider>().enabled = (value == 0) ? false : true;
    }

    private void PlayerSfx(Define.PlayerSfxType type)
    {
        switch (type)
        {
            case Define.PlayerSfxType.MAIN:
                SoundManager.Instance.PlayOneShot("event:/inGAME/SFX_RhythmCombo_Attack_" + (_player.Attack.AttackIndex + 1), transform);
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
