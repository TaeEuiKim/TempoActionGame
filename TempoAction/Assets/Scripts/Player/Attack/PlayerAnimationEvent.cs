using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Unity.VisualScripting;
using System.Threading;
using UnityEngine.UIElements;
using System.Linq;
using System.Net;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private Transform leftHandTrans;
    [SerializeField] private Transform rightHandTrans;
    [SerializeField] private Transform leftFootTrans;
    [SerializeField] private Transform rightFootTrans;
    [SerializeField] private Transform rightHandTrail;
    [SerializeField] private Transform headOrigin;

    [SerializeField] private Material rimShader;
    [SerializeField] private AnimationCurveScriptable animCurve;
    [SerializeField] private GameObject fireObj;

    private CameraController _cameraController;

    [SerializeField] Vector3 tempVec;

    private Vector3 originFire;
    private bool isTurn = false;

    private GameObject dempEffect;
    private GameObject dempInchenEffect;
    private bool isOnDemp;

    private Coroutine tempCoroutine;

    [Header("커맨드 용 카운트")]
    private int commandAttackCount = 0;

    private Vector3 originPos;
    private Vector3 originScale;

    private void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();
    }

    private void LateUpdate()
    {
        if (isTurn && !_player.Ani.GetBool("IsCommand"))
        {
            GameObject effect = ObjectPool.Instance.Spawn("do_disappear", 1);
            effect.transform.position = _player.SkillObject.transform.position;

            TestSound.Instance.PlaySound("Skill1");
            rimShader.SetFloat("_Float", 0f);
            _player.SkillObject.SetActive(true);

            isTurn = false;
        }
    }

    #region Hit
    // 플레이어 타격 위치에 사용하는 이벤트 함수
    private void Hit()
    {
        Collider[] hitMonsters = Physics.OverlapBox(_player.HitPoint.position, _player.HitPoint.localScale / 2, _player.HitPoint.rotation, _player.MonsterLayer | _player.BossLayer);
        Collider[] hitObjects = Physics.OverlapBox(_player.HitPoint.position, _player.HitPoint.localScale / 2, _player.HitPoint.rotation, 1 << 8);

        if (hitMonsters.Length <= 0 && hitObjects.Length <= 0) return;

        Vector3 zPos = new Vector3(0, 0, 0);

        foreach (Collider monsterCollider in hitMonsters)
        {
            Monster monster = monsterCollider.GetComponent<Monster>();

            if (monster.Stat.Hp <= 0)
            {
                continue;
            }

            GameObject hitParticle = null;
            GameObject hitParticle2 = null;
            GameObject hitParticle3 = null;

            if (!_player.Controller.isUltimate && _player.Ani.GetInteger("AtkCount") != 4)
            {
                // 히트 파티클 생성
                hitParticle = ObjectPool.Instance.Spawn("P_Punch", 1);
                hitParticle2 = ObjectPool.Instance.Spawn("P_PunchAttack", 1);
            }
            else if (_player.Controller.isUltimate)
            {
                if (_player.Ani.GetInteger("AtkCount") != 4)
                {
                    hitParticle = ObjectPool.Instance.Spawn("FX_FeverTimePunch", 1);
                }
                hitParticle3 = ObjectPool.Instance.Spawn("FX_PunchBackDust", 1);
            }

            switch (_player.Ani.GetInteger("AtkCount"))
            {
                case 1:
                    if (!_player.Controller.isUltimate)
                    {
                        hitParticle.transform.position = rightHandTrans.position + zPos;
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle2.transform.position = rightHandTrans.position + zPos;
                        hitParticle2.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle.transform.rotation = rightHandTrans.rotation;
                        hitParticle2.transform.rotation = rightHandTrans.rotation;
                    }
                    else
                    {
                        hitParticle.transform.position = rightHandTrans.position + zPos;
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle3.transform.position = _player.transform.position + zPos;
                        hitParticle3.transform.localScale = new Vector3(_player.CharacterModel.localScale.x, 1, 1);

                        hitParticle.transform.rotation = rightHandTrans.rotation;
                    }

                    CameraShaking(0.12f);
                    break;
                case 0:
                case 2:
                case 3:
                    if (!_player.Controller.isUltimate)
                    {
                        hitParticle.transform.position = leftHandTrans.position + zPos;
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle2.transform.position = leftHandTrans.position + zPos;
                        hitParticle2.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle.transform.rotation = leftHandTrans.rotation;
                        hitParticle2.transform.rotation = leftHandTrans.rotation;
                    }
                    else
                    {
                        hitParticle.transform.position = leftHandTrans.position + zPos;
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle3.transform.position = _player.transform.position + zPos;
                        hitParticle3.transform.localScale = new Vector3(_player.CharacterModel.localScale.x, 1, 1);

                        hitParticle.transform.rotation = leftHandTrans.rotation;
                    }

                    CameraShaking(0.12f);
                    break;
                case 4:
                    hitParticle = !_player.Controller.isUltimate ? ObjectPool.Instance.Spawn("FX_PunchAttackSphere", 1) : ObjectPool.Instance.Spawn("FX_FeverTimePunchSphere", 1);

                    hitParticle.transform.position = monsterCollider.ClosestPoint(_player.HitPoint.position) + zPos;

                    if (_player.Controller.isUltimate)
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("FX_PunchBackDust", 1);
                        hitParticle2.transform.position = _player.transform.position;
                        hitParticle2.transform.localScale = new Vector3(_player.CharacterModel.localScale.x, 1, 1);

                        hitParticle3.transform.position = _player.transform.position;
                        hitParticle3.transform.localScale = new Vector3(_player.CharacterModel.localScale.x, 1, 1);
                    }

                    CameraShaking(0.12f);
                    break;
            }

            HitMainTempo(monster);
        }

        foreach (Collider mapObject in hitObjects)
        {
            DestoryObj obj = mapObject.GetComponent<DestoryObj>();

            GameObject hitParticle = null;
            GameObject hitParticle2 = null;
            GameObject hitParticle3 = null;

            if (!_player.Controller.isUltimate && _player.Ani.GetInteger("AtkCount") != 4)
            {
                // 히트 파티클 생성
                hitParticle = ObjectPool.Instance.Spawn("P_Punch", 1);
                hitParticle2 = ObjectPool.Instance.Spawn("P_PunchAttack", 1);
            }
            else if (_player.Controller.isUltimate)
            {
                if (_player.Ani.GetInteger("AtkCount") != 4)
                {
                    hitParticle = ObjectPool.Instance.Spawn("FX_FeverTimePunch", 1);
                }
                hitParticle3 = ObjectPool.Instance.Spawn("FX_PunchBackDust", 1);
            }

            switch (_player.Ani.GetInteger("AtkCount"))
            {
                case 1:
                    if (!_player.Controller.isUltimate)
                    {
                        hitParticle.transform.position = rightHandTrans.position + zPos;
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle2.transform.position = rightHandTrans.position + zPos;
                        hitParticle2.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle.transform.rotation = rightHandTrans.rotation;
                        hitParticle2.transform.rotation = rightHandTrans.rotation;
                    }
                    else
                    {
                        hitParticle.transform.position = rightHandTrans.position + zPos;
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle3.transform.position = _player.transform.position + zPos;
                        hitParticle3.transform.localScale = new Vector3(_player.CharacterModel.localScale.x, 1, 1);

                        hitParticle.transform.rotation = rightHandTrans.rotation;
                    }

                    CameraShaking(0.12f);
                    break;
                case 0:
                case 2:
                case 3:
                    if (!_player.Controller.isUltimate)
                    {
                        hitParticle.transform.position = leftHandTrans.position + zPos;
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle2.transform.position = leftHandTrans.position + zPos;
                        hitParticle2.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle.transform.rotation = leftHandTrans.rotation;
                        hitParticle2.transform.rotation = leftHandTrans.rotation;
                    }
                    else
                    {
                        hitParticle.transform.position = leftHandTrans.position + zPos;
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(_player.CharacterModel.localScale.x, 1, 1));
                        hitParticle3.transform.position = _player.transform.position + zPos;
                        hitParticle3.transform.localScale = new Vector3(_player.CharacterModel.localScale.x, 1, 1);

                        hitParticle.transform.rotation = leftHandTrans.rotation;
                    }

                    CameraShaking(0.12f);
                    break;
                case 4:
                    hitParticle = !_player.Controller.isUltimate ? ObjectPool.Instance.Spawn("FX_PunchAttackSphere", 1) : ObjectPool.Instance.Spawn("FX_FeverTimePunchSphere", 1);

                    hitParticle.transform.position = mapObject.ClosestPoint(_player.HitPoint.position) + zPos;

                    if (_player.Controller.isUltimate)
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("FX_PunchBackDust", 1);
                        hitParticle2.transform.position = _player.transform.position;
                        hitParticle2.transform.localScale = new Vector3(_player.CharacterModel.localScale.x, 1, 1);

                        hitParticle3.transform.position = _player.transform.position;
                        hitParticle3.transform.localScale = new Vector3(_player.CharacterModel.localScale.x, 1, 1);
                    }

                    CameraShaking(0.12f);
                    break;
            }

            HitObject(obj);
        }
    }

    private void CommandHit()
    {
        if (_player.Ani.GetInteger("CommandCount") == 30)
        {
            originPos = _player.HitPoint.position;
            originScale = _player.HitPoint.position;

            _player.HitPoint.localPosition = _player.HitPoint.localPosition - new Vector3(2f, 0, 0);
            _player.HitPoint.localScale = new Vector3(6, 1, 1);
        }

        Collider[] hitMonsters = Physics.OverlapBox(_player.HitPoint.position, _player.HitPoint.localScale / 2, _player.HitPoint.rotation, _player.MonsterLayer | _player.BossLayer);
        Collider[] hitObjects = Physics.OverlapBox(_player.HitPoint.position, _player.HitPoint.localScale / 2, _player.HitPoint.rotation, 1 << 8);

        _player.HitPoint.position = originPos;
        _player.HitPoint.localScale = originScale;

        if (hitMonsters.Length <= 0 && hitObjects.Length <= 0) return;

        foreach (Collider monsterCollider in hitMonsters)
        {
            Monster monster = monsterCollider.GetComponent<Monster>();

            if (monster.Stat.Hp <= 0)
            {
                continue;
            }

            GameObject hitParticle;
            GameObject hitParticle2;
            GameObject hitParticle3;

            ControllTimerScale(0.1f, 0.01f);

            switch (_player.Ani.GetInteger("CommandCount"))
            {
                case 1:
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("Smash1_Hit");

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
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("Smash2_Hit");

                    hitParticle = ObjectPool.Instance.Spawn("P_MainChar_SmashShoryukenAttack", 1);

                    hitParticle.transform.position = _player.transform.position + new Vector3(1.3f * -_player.CharacterModel.localScale.x, 1f);

                    if (_player.CharacterModel.localScale.x < 0)
                    {
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(-1, -1, 1));
                    }
                    break;
                case 3:
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("SmashHit1");

                    bool leftDir = _player.IsLeftDirection();
                    if (commandAttackCount == 0)
                    {
                        hitParticle = ObjectPool.Instance.Spawn("P_PunchAttack2-1", 1);

                        if (leftDir)
                        {
                            hitParticle.transform.position = _player.transform.position + new Vector3(-0.7f, 1.1f, -1);
                        }
                        else
                        {
                            hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(-1, 1, 1));
                            hitParticle.transform.position = _player.transform.position + new Vector3(0.7f, 1f, -1);
                        }

                        commandAttackCount++;
                    }
                    else if (commandAttackCount == 1)
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("P_PunchAttack2-1(2)", 1);

                        if (leftDir)
                        {
                            hitParticle2.transform.position = _player.transform.position + new Vector3(-0.7f, 1.1f, -1);
                        }
                        else
                        {
                            hitParticle2.transform.position = _player.transform.position + new Vector3(0.7f, 1f, -1);
                        }

                        commandAttackCount++;
                    }
                    else if (commandAttackCount == 2)
                    {
                        hitParticle3 = ObjectPool.Instance.Spawn("P_SmashHit2-1", 1);

                        hitParticle3.transform.position = monsterCollider.ClosestPoint(monsterCollider.transform.position) + new Vector3(0, 1f, -1);

                        commandAttackCount = 0;
                    }
                    break;
                case 11:
                    TestSound.Instance.PlaySound("SmashHit1");

                    CameraShaking(0.12f);

                    if (_player.IsLeftDirection())
                    {
                        hitParticle = ObjectPool.Instance.Spawn("P_PunchAttack1-2", 1);

                        hitParticle.transform.position = _player.transform.position + new Vector3(-0.9f, 0.5f, -1.2f);
                    }
                    else
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("P_PunchAttack1-2_mirror", 1);

                        hitParticle2.transform.position = _player.transform.position + new Vector3(0.9f, 0.5f, -1.2f);
                    }

                    break;
                case 12:
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("SmashHit2");

                    if (_player.IsLeftDirection())
                    {
                        hitParticle = ObjectPool.Instance.Spawn("P_MainChar_SmashShoryukenAttack1-2", 1);

                        hitParticle.transform.position = _player.transform.position + new Vector3(-0.7f, 1.5f, -1);
                    }
                    else
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("P_MainChar_Smash1-2 mirror", 1);

                        hitParticle2.transform.position = _player.transform.position + new Vector3(0.7f, 1.5f, -1);
                    }
                    break;
                case 13:
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("SmashHit1");
                    break;
                case 22:
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("SmashHit2");
                    if (_player.IsLeftDirection())
                    {
                        hitParticle = ObjectPool.Instance.Spawn("P_MainChar_ParryingSkillAttack", 1);

                        hitParticle.transform.position = _player.transform.position + new Vector3(-0.3f, 0.7f);
                    }
                    else
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("P_MainChar_ParryingSkillAttackMirror", 1);

                        hitParticle2.transform.position = _player.transform.position + new Vector3(0.3f, 0.7f);
                    }
                    break;
                case 23:
                    TestSound.Instance.PlaySound("SmashHit2");

                    hitParticle = ObjectPool.Instance.Spawn("FX_Smash2-3", 1);
                    CameraShaking(0.12f);

                    hitParticle.transform.position = monsterCollider.ClosestPoint(_player.transform.position);
                    break;
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                    TestSound.Instance.PlaySound("SmashHit1");
                    hitParticle = ObjectPool.Instance.Spawn("FX_PC_OraoraPunch", 0.5f);
                    CameraShaking(0.12f);

                    if (_player.IsLeftDirection())
                    {
                        hitParticle.transform.position = _player.transform.position + new Vector3(1f, 0.7f, 4f);
                        hitParticle.transform.localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        hitParticle.transform.position = _player.transform.position + new Vector3(-0.3f, 0.7f, 4f);
                        hitParticle.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    break;
                case 30:
                    CameraShaking(0.12f);
                    TestSound.Instance.PlaySound("Skill3_Final");
                    TestSound.Instance.PlaySound("RushFinishHit");
                    hitParticle = ObjectPool.Instance.Spawn("energy_hit_oraora", 1);

                    if (_player.IsLeftDirection())
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("P_MainChar_oraora_attack", 1);
                        hitParticle3 = ObjectPool.Instance.Spawn("chen_burn_flame(ora)", 1.5f);

                        hitParticle.transform.position = leftHandTrans.position + new Vector3(-1.6f, -0.7f, -1);
                        hitParticle2.transform.position = leftHandTrans.position + new Vector3(-1.6f, -0.7f, -1);
                        hitParticle3.transform.position = monsterCollider.transform.position;
                    }
                    else
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("P_MainChar_oraora_attack_mirror", 1);
                        hitParticle3 = ObjectPool.Instance.Spawn("chen_burn_flame(ora)_mirror", 1.5f);

                        hitParticle.transform.position = leftHandTrans.position + new Vector3(1.6f, -0.7f, -1);
                        hitParticle2.transform.position = leftHandTrans.position + new Vector3(1.6f, -0.7f, -1);
                        hitParticle3.transform.position = monsterCollider.transform.position;
                    }
                    break;
                case 101:
                    hitParticle = ObjectPool.Instance.Spawn("FX_Smash3", 1);

                    hitParticle.transform.position = monsterCollider.ClosestPoint(monster.transform.position) + new Vector3(-0.2f, 0);
                    break;
                case 201:
                case 202:

                    break;
            }

            HitMainTempo(monster);
        }

        foreach (Collider mapObject in hitObjects)
        {
            DestoryObj obj = mapObject.GetComponent<DestoryObj>();

            GameObject hitParticle;
            GameObject hitParticle2;
            GameObject hitParticle3;

            switch (_player.Ani.GetInteger("CommandCount"))
            {
                case 1:
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("Smash1_Hit");

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
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("SmashHit2");

                    hitParticle = ObjectPool.Instance.Spawn("P_MainChar_SmashShoryukenAttack", 1);

                    hitParticle.transform.position = _player.transform.position + new Vector3(1.3f * -_player.CharacterModel.localScale.x, 1f);

                    if (_player.CharacterModel.localScale.x < 0)
                    {
                        hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(-1, -1, 1));
                    }
                    break;
                case 3:
                    CameraShaking(0.12f);

                    bool leftDir = _player.IsLeftDirection();
                    TestSound.Instance.PlaySound("SmashHit1");

                    if (commandAttackCount == 0)
                    {
                        hitParticle = ObjectPool.Instance.Spawn("P_PunchAttack2-1", 1);

                        if (leftDir)
                        {
                            hitParticle.transform.position = _player.transform.position + new Vector3(-0.5f, 1f, -1);
                        }
                        else
                        {
                            hitParticle.GetComponent<FlipSlash>().OnFlip(new Vector3(-1, 1, 1));
                            hitParticle.transform.position = _player.transform.position + new Vector3(0.7f, 1f, -1);
                        }

                        commandAttackCount++;
                    }
                    else if (commandAttackCount == 1)
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("P_PunchAttack2-1(2)", 1);

                        if (leftDir)
                        {
                            hitParticle2.transform.position = _player.transform.position + new Vector3(-0.5f, 1f, -1);
                        }
                        else
                        {
                            hitParticle2.transform.position = _player.transform.position + new Vector3(0.7f, 1f, -1);
                        }

                        commandAttackCount++;
                    }
                    else if (commandAttackCount == 2)
                    {
                        hitParticle3 = ObjectPool.Instance.Spawn("P_SmashHit2-1", 1);

                        hitParticle3.transform.position = mapObject.ClosestPoint(mapObject.transform.position) + new Vector3(0, 1f, -1);

                        commandAttackCount = 0;
                    }
                    break;
                case 11:
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("SmashHit1");

                    if (_player.IsLeftDirection())
                    {
                        hitParticle = ObjectPool.Instance.Spawn("P_PunchAttack1-2", 1);

                        hitParticle.transform.position = _player.transform.position + new Vector3(-0.9f, 0.5f, -1.2f);
                    }
                    else
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("P_PunchAttack1-2_mirror", 1);

                        hitParticle2.transform.position = _player.transform.position + new Vector3(0.9f, 0.5f, -1.2f);
                    }

                    break;
                case 12:
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("SmashHit2");

                    if (_player.IsLeftDirection())
                    {
                        hitParticle = ObjectPool.Instance.Spawn("P_MainChar_SmashShoryukenAttack1-2", 1);
                        
                        hitParticle.transform.position = _player.transform.position + new Vector3(-0.7f, 1.5f, -1);
                    }
                    else
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("P_MainChar_Smash1-2 mirror", 1);

                        hitParticle2.transform.position = _player.transform.position + new Vector3(0.7f, 1.5f, -1);
                    }
                    break;
                case 13:
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("SmashHit1");
                    break;
                case 22:
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("SmashHit2");

                    if (_player.IsLeftDirection())
                    {
                        hitParticle = ObjectPool.Instance.Spawn("P_MainChar_ParryingSkillAttack", 1);

                        hitParticle.transform.position = _player.transform.position + new Vector3(-0.3f, 0.7f);
                    }
                    else
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("P_MainChar_ParryingSkillAttackMirror", 1);

                        hitParticle2.transform.position = _player.transform.position + new Vector3(0.3f, 0.7f);
                    }
                    break;
                case 23:
                    CameraShaking(0.12f);

                    TestSound.Instance.PlaySound("SmashHit2");

                    hitParticle = ObjectPool.Instance.Spawn("FX_Smash2-3", 1);

                    hitParticle.transform.position = mapObject.ClosestPoint(_player.transform.position);
                    break;
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                    CameraShaking(0.12f);
                    TestSound.Instance.PlaySound("SmashHit1");
                    hitParticle = ObjectPool.Instance.Spawn("FX_PC_OraoraPunch", 0.5f);

                    if (_player.IsLeftDirection())
                    {
                        hitParticle.transform.position = _player.transform.position + new Vector3(1f, 0.7f);
                        hitParticle.transform.localScale = new Vector3(1, 1, 1);
                        hitParticle2 = ObjectPool.Instance.Spawn("orora_dust3", 1);
                        hitParticle2.transform.position = _player.transform.position + new Vector3(-0.5f, 0.2f);
                    }
                    else
                    {
                        hitParticle.transform.position = _player.transform.position + new Vector3(-0.3f, 0.7f);
                        hitParticle.transform.localScale = new Vector3(-1, 1, 1);
                        hitParticle2 = ObjectPool.Instance.Spawn("orora_dust3_2", 1);
                        hitParticle2.transform.position = _player.transform.position + new Vector3(0.5f, 0.2f);
                    }
                    break;
                case 30:
                    CameraShaking(0.12f);
                    hitParticle = ObjectPool.Instance.Spawn("energy_hit_oraora", 1);
                    TestSound.Instance.PlaySound("RushFinishHit");
                    TestSound.Instance.PlaySound("SmashHit2");

                    if (_player.IsLeftDirection())
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("P_MainChar_oraora_attack", 1);
                        hitParticle3 = ObjectPool.Instance.Spawn("chen_burn_flame(ora)", 1.5f);

                        hitParticle.transform.position = leftHandTrans.position + new Vector3(-1.6f, -0.7f, -1);
                        hitParticle2.transform.position = leftHandTrans.position + new Vector3(-1.6f, -0.7f, -1);
                        hitParticle3.transform.position = mapObject.transform.position;
                    }
                    else
                    {
                        hitParticle2 = ObjectPool.Instance.Spawn("P_MainChar_oraora_attack_mirror", 1);
                        hitParticle3 = ObjectPool.Instance.Spawn("chen_burn_flame(ora)_mirror", 1.5f);

                        hitParticle.transform.position = leftHandTrans.position + new Vector3(1.6f, -0.7f, -1);
                        hitParticle2.transform.position = leftHandTrans.position + new Vector3(1.6f, -0.7f, -1);
                        hitParticle3.transform.position = mapObject.transform.position;
                    }
                    break;
                case 101:
                    hitParticle = ObjectPool.Instance.Spawn("FX_Smash3", 1);

                    hitParticle.transform.position = mapObject.ClosestPoint(obj.transform.position) + new Vector3(-0.2f, 0);
                    break;
                case 201:

                    break;
            }

            HitObject(obj);
        }
    }

    private void UltimateHit()
    {
        Collider[] hitMonsters = Physics.OverlapBox(_player.HitPoint.position, _player.HitPoint.localScale / 2, _player.HitPoint.rotation, _player.MonsterLayer | _player.BossLayer);
        Collider[] hitObjects = Physics.OverlapBox(_player.HitPoint.position, _player.HitPoint.localScale / 2, _player.HitPoint.rotation, 1 << 8);

        if (hitMonsters.Length <= 0 && hitObjects.Length <= 0) return;

        foreach (Collider monsterCollider in hitMonsters)
        {
            Monster monster = monsterCollider.GetComponent<Monster>();

            HitMainTempo(monster);
        }

        foreach (Collider mapObject in hitObjects)
        {
            DestoryObj obj = mapObject.GetComponent<DestoryObj>();

            HitObject(obj);
        }
    }

    private void HitMainTempo(Monster monster)
    {
        float commandDamage = 0f;
        int count = _player.Ani.GetInteger("CommandCount");

        ControllTimerScale(0.1f, 0.005f);

        if (count != 0)
        {
            _player.View.UpdateUltimateGauge(_player._skillCommand.GetUltimateGauge(count) / _player.PlayerSt._maxUltimateGauge);
            commandDamage = _player._skillCommand.GetDamage(count);
        }
        else
        {
            _player.View.UpdateUltimateGauge(_player.fillAttackUltimateGauge / _player.PlayerSt._maxUltimateGauge);
        }

        // 메인 템포일 때 데미지 처리
        PlayerSfx(Define.PlayerSfxType.MAIN);

        monster.TakeDamage(_player.GetTotalDamage() + commandDamage);
    }

    private void HitObject(DestoryObj obj)
    {
        int count = _player.Ani.GetInteger("CommandCount");

        if (count != 0)
        {
            _player.View.UpdateUltimateGauge(_player._skillCommand.GetUltimateGauge(count) / _player.PlayerSt._maxUltimateGauge);
        }
        else
        {
            _player.View.UpdateUltimateGauge(_player.fillAttackUltimateGauge / _player.PlayerSt._maxUltimateGauge);
        }

        obj.TakeDamage(_player.GetTotalDamage());
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

    // 공격 애니메이션 끝에 추가하는 이벤트 함수
    private void Finish(float delay)
    {
        _player.Attack.CheckDelay = delay;

        _player.Attack.ChangeCurrentAttackState(Define.AttackState.CHECK);
        if (dempEffect != null && dempEffect.gameObject.activeSelf)
        {
            GameObject effect = ObjectPool.Instance.Spawn("do_disappear", 1);
            effect.transform.position = _player.SkillObject.transform.position;

            ObjectPool.Instance.Remove(dempEffect);
            ObjectPool.Instance.Remove(dempInchenEffect);

            TestSound.Instance.PlaySound("Skill1");
            rimShader.SetFloat("_Float", 0f);
            _player.SkillObject.SetActive(true);

            isOnDemp = false;
        }

        if (fireObj.activeSelf)
        {
            fireObj.SetActive(false);
        }

        //_player.Ani.SetBool("IsCommand", false);
        //_player.Ani.SetInteger("CommandCount", 0);
        //PlayerInputManager.Instance.ResetCommandKey();
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
            _player.Ani.SetBool("IsRock", true);
        }
    }

    private void JumpFinish()
    {
        if (_player.Rb.isKinematic)
        {
            _player.Ani.SetFloat("VerticalSpeed", 0);
            _player.Rb.isKinematic = false;
            _player.Ani.SetBool("IsRock", false);
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
        PlayerInputManager.Instance.ResetCommandKey();
    }

    // 공격 사거리 안에 적이 있으면 적 앞으로 이동하는 이벤트 함수
    private void MoveToClosestMonster(float duration)
    {
        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y + 0.5f, transform.parent.position.z);
        Vector3 rayDirection = _player.IsLeftDirection() ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);

        // 레이캐스트 실행
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, _player.Attack.CurrentTempoData.distance, _player.MonsterLayer))
        {
            float closestMonsterX = hit.point.x + (-rayDirection.x * 0.2f);
            transform.parent.DOMoveX(closestMonsterX, duration);
        }
    }

    private void MoveCommandAttack(float moveDistance)
    {
        _player.Ani.SetFloat("Speed", 0);

        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y + 0.5f, transform.parent.position.z);
        Vector3 rayDirection = _player.IsLeftDirection() ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);

        float dis = 2;
        if (_player.Attack.CurrentTempoData != null)
        {
            dis = _player.Attack.CurrentTempoData.distance;
        }

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitPos, moveDistance, _player.BlockLayer))
        {
            float closestMonsterX = hitPos.point.x + (-rayDirection.x * 0.5f);
            transform.parent.DOMoveX(closestMonsterX, 0.3f);
        }
        else
        {
            switch (_player.Ani.GetInteger("CommandCount"))
            {
                case 1:
                case 2:
                case 11:
                case 12:
                case 13:
                case 22:
                    transform.parent.DOMoveX(transform.parent.position.x - (moveDistance * _player.CharacterModel.localScale.x), 0.3f);
                    break;
                case 4:
                    Vector3 targetPos = new Vector3(transform.parent.position.x - (moveDistance * _player.CharacterModel.localScale.x)
                                                   , transform.parent.position.y + 4f, 0);
                    transform.parent.DOMove(targetPos, 0.7f);
                    break;
                case 23:
                    transform.parent.DOKill();
                    transform.parent.DOMoveX(transform.parent.position.x - (moveDistance * _player.CharacterModel.localScale.x), 0.1f);
                    break;
            }
        }
    }

    private void MoveAttack()
    {
        _player.Ani.SetFloat("Speed", 0);

        Vector3 rayOrigin = new Vector3(transform.parent.position.x, transform.parent.position.y + 0.5f, transform.parent.position.z);
        Vector3 rayDirection = _player.IsLeftDirection() ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);

        float dis = 2f;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitPos, dis, _player.BlockLayer))
        {
            float closestMonsterX = hitPos.point.x + (-rayDirection.x * 0.5f);
            transform.parent.DOMoveX(closestMonsterX, 0.3f);
        }
        else
        {
            switch (_player.Ani.GetInteger("AtkCount"))
            {
                case 0:
                case 1:
                default:
                    transform.parent.DOMoveX(transform.parent.position.x + animCurve.curves[0].xDis * -_player.CharacterModel.localScale.x, animCurve.curves[0].curveTime)
                                    .SetEase(animCurve.curves[0].xCurve);
                    break;
                case 2:
                    transform.parent.DOMoveX(transform.parent.position.x + animCurve.curves[1].xDis * -_player.CharacterModel.localScale.x, animCurve.curves[1].curveTime)
                                    .SetEase(animCurve.curves[1].xCurve);
                    break;
            }
        }
    }

    private void CheckCommand(int SkillId)
    {
        AnimatorStateInfo stateInfo = _player.Ani.GetCurrentAnimatorStateInfo(0); // 레이어 0에서 재생 중인 애니메이션 상태

        // 현재 애니메이션의 남은 시간 계산
        float animationLength = stateInfo.length; // 애니메이션 전체 길이
        float currentPlayTime = stateInfo.normalizedTime * animationLength; // 현재 재생된 시간 (normalizedTime은 0에서 1 사이의 값)

        float timeRemaining = animationLength - currentPlayTime; // 남은 시간 계산
        
        _player.Ani.SetBool("IsCommand", true);
        if (_player.Ani.GetBool("isGrounded"))
        {
            _player.Attack.ChangeCurrentAttackState(Define.AttackState.ATTACK);
        }

        if (_player.Ani.GetInteger("CommandCount") == 0)
        {
            _player.CommandController.StartCommandTime(timeRemaining - 0.2f, SkillId, false);
        }
        else
        {
            _player.CommandController.StartCommandTime(_player.PlayerSt.KeyInputTime, SkillId, false);
        }
    }

    private void StepCheckCommand(int SkillId)
    {
        commandAttackCount = 0;
        fireObj.SetActive(false);

        if (dempEffect != null && dempEffect.gameObject.activeSelf)
        {
            GameObject effect = ObjectPool.Instance.Spawn("do_disappear", 1);
            effect.transform.position = _player.SkillObject.transform.position;

            ObjectPool.Instance.Remove(dempEffect);
            ObjectPool.Instance.Remove(dempInchenEffect);

            TestSound.Instance.PlaySound("Skill1");
            rimShader.SetFloat("_Float", 0f);
            _player.SkillObject.SetActive(true);

            isOnDemp = false;
        }

        _player.Ani.SetBool("IsCommand", true);
        PlayerInputManager.Instance.isDashCommand = true;

        _player.CommandController.StartCommandTime(0.3f, SkillId, true);
        StartCoroutine(CheckStepCommandTime());
    }

    private IEnumerator CheckStepCommandTime()
    {
        yield return new WaitForSeconds(0.2f);

        if (_player.Ani.GetBool("IsCommand") && _player.Ani.GetInteger("CommandCount") == 0)
        {
            PlayerInputManager.Instance.ResetCommandKey();
            PlayerInputManager.Instance.downArrow = false;
            _player.Ani.SetBool("IsCommand", false);
        }
    }

    private void ChangeAttackState()
    {
        _player.Controller.SetBackDash();
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.ATTACK);
    }

    private void ChangeNormalAttackState(int attackCount)
    {
        if (tempCoroutine != null)
        {
            StopCoroutine(tempCoroutine);
        }
        _player.Controller.SetBackDash();
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.ATTACK);
        PlayerInputManager.Instance.isKeyZ = true;
        tempCoroutine = StartCoroutine(CheckNormalAttackKey(attackCount));
    }

    private IEnumerator CheckNormalAttackKey(int attackCount)
    {
        float timer = 0f;

        while (timer <= 1.3f)
        {
            timer += Time.deltaTime;

            if (PlayerInputManager.Instance.attack)
            {
                PlayerInputManager.Instance.isKeyZ = false;
                PlayerInputManager.Instance.attack = false;
                _player.Attack.AttackMainTempo();
                _player.Attack.AttackMainTempo();

                _player.Ani.SetInteger("CommandCount", 0);

                yield break;
            }

            yield return null;
        }
    }

    #region Effect

    private void LeftFootEffect()
    {
        GameObject effect = ObjectPool.Instance.Spawn("FX_Walk", 1);

        if(effect == null) { return; }

        effect.transform.position = leftFootTrans.position + new Vector3(0, -0.2f);

        if (_player.Controller._isGrounded)
        {
            switch (Player.curScene)
            {
                case "StartScene":
                    if (_player.transform.position.x < -131)
                    {
                        SoundManager.Instance.PlayOneShot("event:/SFX_BG_PF_Concrete", _player.transform);
                    }
                    else if (_player.transform.position.x > -131)
                    {
                        SoundManager.Instance.PlayOneShot("event:/SFX_BG_PF_Steel", _player.transform);
                    }
                    break;
                case "MiddleBossStage":
                    SoundManager.Instance.PlayOneShot("event:/SFX_BG_PF_Grating", _player.transform);
                    break;
            }
        }
    }

    private void RightFootEffect()
    {
        GameObject effect = ObjectPool.Instance.Spawn("FX_Walk", 1);

        if (effect == null) { return; }

        effect.transform.position = rightFootTrans.position + new Vector3(0, -0.2f);

        if (_player.Controller._isGrounded)
        {
            switch (Player.curScene)
            {
                case "StartScene":
                    if (_player.transform.position.x < -131)
                    {
                        SoundManager.Instance.PlayOneShot("event:/SFX_BG_PF_Concrete", _player.transform);
                    }
                    else if (_player.transform.position.x > -131)
                    {
                        SoundManager.Instance.PlayOneShot("event:/SFX_BG_PF_Steel", _player.transform);
                    }
                    break;
                case "MiddleBossStage":
                    SoundManager.Instance.PlayOneShot("event:/SFX_BG_PF_Grating", _player.transform);
                    break;
            }
        }
    }

    private void TurnFire(float appearTime)
    {
        if (_player.Controller.isUltimate)
        {
            return;
        }

        if (isTurn)
        {
            StopCoroutine(TurnOnFire(appearTime));
        }

        if (!isOnDemp)
        {
            GameObject disappearEffect = ObjectPool.Instance.Spawn("do_disappear", 1f);
            disappearEffect.transform.position = _player.SkillObject.transform.position;
            TestSound.Instance.PlaySound("Skill1");
        }

        rimShader.SetFloat("_Float", 1.84f);
        _player.SkillObject.SetActive(false);

        StartCoroutine(TurnOnFire(appearTime));
    }

    private IEnumerator TurnOnFire(float appearTime)
    {
        isTurn = true;

        if (appearTime == 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(appearTime);

        if (isTurn)
        {
            GameObject effect = ObjectPool.Instance.Spawn("do_disappear", 1);
            effect.transform.position = _player.SkillObject.transform.position;

            TestSound.Instance.PlaySound("Skill1");
            rimShader.SetFloat("_Float", 0f);
            _player.SkillObject.SetActive(true);

            isTurn = false;
        }

        yield return null;
    }

    private void TurnOnPlayerEffect(string effectName)
    {
        float dis = 0;

        GameObject effect;
        GameObject effect2;
        GameObject effect3;

        switch (effectName)
        {
            case "chen_rush":
                effect = ObjectPool.Instance.Spawn(effectName, 1.5f);
                if (_player.IsLeftDirection())
                {
                    effect.transform.localScale = new Vector3(1, 1, 1);
                    effect.transform.position = _player.transform.position + new Vector3(7, 1f);
                }
                else
                {
                    effect.transform.localScale = new Vector3(-1, 1, 1);
                    effect.transform.position = _player.transform.position + new Vector3(-7, 1f);
                }
                break;
            case "PRush":
                if (_player.IsLeftDirection())
                {
                    effect = ObjectPool.Instance.Spawn("FX_SwordRushLeft@Player", 1);
                    effect.transform.position = _player.transform.position + new Vector3(0, 0.5f);
                }
                else
                {
                    effect = ObjectPool.Instance.Spawn("FX_SwordRushRight@Player", 1);
                    effect.transform.position = _player.transform.position + new Vector3(0, 0.5f);
                }
                break;
            case "in_chen":
                effect = ObjectPool.Instance.Spawn(effectName, 1.25f, rightHandTrans);
                effect.transform.position = rightHandTrans.position;
                effect.transform.rotation = rightHandTrans.rotation;
                break;
            case "P_ShoryukenJumpDust":
                effect = ObjectPool.Instance.Spawn("P_ShoryukenJumpDust", 1);

                effect.transform.position = _player.transform.position + new Vector3(0.3f * -_player.CharacterModel.localScale.x, 0);
                break;
            case "chen_burn_flame-round1":
                effect2 = ObjectPool.Instance.Spawn(effectName, 1f);
                effect2.transform.position = _player.transform.position + new Vector3(-2f, 0);
                break;
            case "chen_burn_flame-round2":
                effect = ObjectPool.Instance.Spawn(effectName, 1f);
                effect.transform.position = _player.transform.position + new Vector3(2f, 0);
                break;
            case "chen_fall":
                effect = ObjectPool.Instance.Spawn(effectName, 1f);
                effect.transform.position = _player.transform.position;
                break;
            case "chen_burn_flame_orora (1)":
                if (!isOnDemp)
                {
                    isOnDemp = true;

                    dempInchenEffect = ObjectPool.Instance.Spawn("in_chen", 0, rightHandTrans);
                    dempInchenEffect.transform.position = rightHandTrans.position;
                    dempInchenEffect.transform.rotation = rightHandTrans.rotation;

                    dempEffect = ObjectPool.Instance.Spawn("chen_burn_flame_orora (1)");

                    dempEffect.transform.position = _player.transform.position + new Vector3(0.2f * -_player.transform.localScale.x, 0);
                }
                break;
            case "P_Punch2-1":
                effect = ObjectPool.Instance.Spawn(effectName, 1f);
                if (_player.IsLeftDirection())
                {
                    effect.transform.position = _player.transform.position + new Vector3(-1.2f, 1f);
                }
                else
                {
                    effect.GetComponent<FlipSlash>().OnFlip(new Vector3(-1, 1, 1));
                    effect.transform.position = _player.transform.position + new Vector3(-1f, 1f);
                }
                break;
            case "P_Punch2-1(2)":
                if (_player.IsLeftDirection())
                {
                    effect = ObjectPool.Instance.Spawn(effectName, 1f);
                    effect.transform.position = _player.transform.position + new Vector3(-1.2f, 1f);
                }
                else
                {
                    effect = ObjectPool.Instance.Spawn("P_Punch_left2-1(2)");
                    effect.transform.position = _player.transform.position + new Vector3(0, 0.8f);
                }
                break;
            case "P_Punch 1-2_low":
                if (_player.IsLeftDirection())
                {
                    effect = ObjectPool.Instance.Spawn(effectName, 1f);
                    effect.transform.position = _player.transform.position + new Vector3(-1.7f, 0.9f, -1);
                }
                else
                {
                    effect = ObjectPool.Instance.Spawn("P_Punch 1-2_low_mirror", 1);
                    effect.transform.position = _player.transform.position + new Vector3(-0.7f, 0.9f, -1);
                }
                break;
            case "P_Punch 1-2_high":
                if (_player.IsLeftDirection())
                {
                    effect = ObjectPool.Instance.Spawn(effectName, 1f);
                    effect.transform.position = _player.transform.position + new Vector3(-1.8f, 1f, -1);
                }
                else
                {
                    effect = ObjectPool.Instance.Spawn("P_Punch 1-2_high_mirror", 1f);
                    effect.transform.position = _player.transform.position + new Vector3(-0.4f, 1f, -1);
                }
                break;
            case "P_Smash22_Punch1":
                if (_player.IsLeftDirection())
                {
                    effect = ObjectPool.Instance.Spawn(effectName, 1f);
                    //effect2 = ObjectPool.Instance.Spawn("P_Smash22_Attack1", 1);

                    effect.transform.position = _player.transform.position + new Vector3(-0.3f, 1.4f, -1);
                    //effect2.transform.position = _player.transform.position + new Vector3(-0.6f, 1.4f, -1);
                }
                else
                {
                    effect = ObjectPool.Instance.Spawn("P_Smash22_Punch1_Right", 1f);
                    //effect2 = ObjectPool.Instance.Spawn("P_Smash22_Attack1_Right", 1);

                    effect.transform.position = _player.transform.position + new Vector3(0.3f, 1.4f, -1);
                    //effect2.transform.position = _player.transform.position + new Vector3(1f, 1.5f, -1);
                }
                break;
            case "P_Smash22_Punch2":
                if (_player.IsLeftDirection())
                {
                    effect = ObjectPool.Instance.Spawn(effectName, 1f);
                    effect2 = ObjectPool.Instance.Spawn("P_Smash22_Attack2", 1f);

                    effect.transform.position = _player.transform.position + new Vector3(-0.3f, 1.4f, -1);
                    effect2.transform.position = _player.transform.position + new Vector3(-0.8f, 0f, -1);
                }
                else
                {
                    effect = ObjectPool.Instance.Spawn("P_Smash22_Punch2_Right", 1f);
                    effect2 = ObjectPool.Instance.Spawn("P_Smash22_Attack2_Right", 1f);

                    effect.transform.position = _player.transform.position + new Vector3(0.3f, 1.4f, -1);
                    effect2.transform.position = _player.transform.position + new Vector3(0.8f, 0f, -1);
                }
                break;
            case "P_Punch _smash2-3":
                if (_player.IsLeftDirection())
                {
                    effect = ObjectPool.Instance.Spawn(effectName, 1f);
                    effect2 = ObjectPool.Instance.Spawn("pc_dust2-3", 1f);

                    effect.transform.position = _player.transform.position + new Vector3(-0.7f, 0.9f, -1);
                    effect2.transform.position = _player.transform.position;
                }
                else
                {
                    effect = ObjectPool.Instance.Spawn("P_Punch _smash2-3 (mirror", 1f);
                    effect2 = ObjectPool.Instance.Spawn("pc_dust2-3", 1f);

                    effect.transform.position = _player.transform.position + new Vector3(0.7f, 0.9f, -1);
                    effect2.transform.position = _player.transform.position;
                }
                break;
            case "FireObj":
                fireObj.SetActive(true);
                break;
            case "FireObjOff":
                fireObj.SetActive(false);
                break;
            case "P_Punch _Parrying":
                if (_player.IsLeftDirection())
                {
                    effect = ObjectPool.Instance.Spawn(effectName, 1);

                    effect.transform.position = _player.transform.position + new Vector3(0, 0.7f);
                }
                else
                {
                    effect = ObjectPool.Instance.Spawn("P_Punch _Parrying_mirror", 1);

                    effect.transform.position = _player.transform.position + new Vector3(0, 0.7f);
                }
                break;
            case "P_Punch_smash_oraora":
                if (_player.IsLeftDirection())
                {
                    effect = ObjectPool.Instance.Spawn(effectName, 1);

                    effect.GetComponent<FlipSlash>().OnFlip(new Vector3(1, 1, 1));

                    effect.transform.position = _player.transform.position + new Vector3(0, 1f);
                }
                else
                {
                    effect = ObjectPool.Instance.Spawn(effectName, 1);
                    effect.GetComponent<FlipSlash>().OnFlip(new Vector3(-1, 1, 1));

                    effect.transform.position = _player.transform.position + new Vector3(0, 1f);
                }
                break;
            case "orora_dust3_33":
                effect = ObjectPool.Instance.Spawn(effectName, 1);

                if (_player.IsLeftDirection())
                {
                    effect.transform.position = _player.transform.position;
                }
                else
                {
                    effect.transform.rotation = new Quaternion(effect.transform.rotation.x, -71.597f, effect.transform.rotation.z, effect.transform.rotation.w);
                }
                break;
            case "oraora_dust (3)_33":
                effect = ObjectPool.Instance.Spawn(effectName, 1);

                if (_player.IsLeftDirection())
                {
                    effect.transform.position = _player.transform.position;
                }
                else
                {
                    effect.transform.rotation = new Quaternion(effect.transform.rotation.x, -71.597f, effect.transform.rotation.z, effect.transform.rotation.w);
                }
                break;
        }
    }

    private void SetOniFireFollow(int isFollow)
    {
        if (isFollow == 0)
        {
            _player.isRockFireObj = true;
            StartCoroutine(FollowOniFire());
        }
        else
        {
            _player.isRockFireObj = false;

            if (_player.IsLeftDirection())
            {
                _player.SkillObject.transform.DOLocalMoveX(0.68f, 0.3f);
            }
            else
            {
                _player.SkillObject.transform.DOLocalMoveX(-0.68f, 0.3f);
            }
        }
    }

    private IEnumerator FollowOniFire()
    {
        float dir = 1f;
        if (!_player.IsLeftDirection())
        {
            dir = 0;
        }

        while (_player.isRockFireObj)
        {
            if (!_player.IsLeftDirection() && dir < 1)
            {
                dir += Time.fixedDeltaTime;
            }

            _player.SkillObject.transform.localPosition = headOrigin.transform.localPosition + new Vector3(dir, 0.5f);

            yield return null;
        }
    }

    #endregion

    private void ControllTimerScale(float scale, float time)
    {
        Time.timeScale = scale;
        StartCoroutine(StartRevertTimeScale(time));
    }

    private IEnumerator StartRevertTimeScale(float time)
    {
        yield return new WaitForSeconds(time);

        Time.timeScale = 1;
    }

    //타임라인 실행 함수
    private void StartTimeline(string name)
    {
        TimelineManager.Instance.PlayTimeline(name);
    }

    private void LockPlayer(int isLock)
    {
        bool _lock = isLock == 0 ? false : true;
        _player.PlayerSt.IsKnockedBack = _lock;
    }

    private void TurnOnCounter()
    {
        _player.Attack.ChangeCurrentAttackState(Define.AttackState.ATTACK);
        _player.isCounter = true;
    }

    private void PlayerSfx(Define.PlayerSfxType type)
    {
        switch (type)
        {
            case Define.PlayerSfxType.MAIN:
                SoundManager.Instance.PlayOneShot("event:/Atk_Hit", transform);
                break;
            case Define.PlayerSfxType.POINT:
                SoundManager.Instance.PlayOneShot("event:/Atk_Hit", transform);
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
