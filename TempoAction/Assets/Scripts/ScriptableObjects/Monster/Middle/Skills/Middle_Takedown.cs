using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Takedown", menuName = "ScriptableObjects/MiddleMonster/Skill/Takedown", order = 1)]
public class Middle_Takedown : Middle_Skill
{
    private float _coolTime = 0f;
    private float count = 0f;
    private bool _isHit = false;
    private Vector3 orginPoint;
    private Vector3 originSize;

    [Header("Hit 포지션")]
    [SerializeField] private Vector3 _hitPoint;
    [Header("Hit 스케일")]
    [SerializeField] private Vector3 _hitScale;
    [SerializeField] private float _knockBackPower;         
    [SerializeField] private float _knockBackDuration;
    [SerializeField] int _attackCount;

    [Header("피니쉬 공격 데미지 체력")]
    [SerializeField] float _finishDamage;

    public override void Init(MiddleMonster monster)
    {
        base.Init(monster);

        _coolTime = 0f;
        count = 0f;
        _isHit = false;
    }

    public override void Check()
    {
        if (IsCompleted) return;

        if (_coolTime >= _info.coolTime) // 쿨타임 확인
        {
            if (Vector2.Distance(_monster.Player.position, _monster.transform.position) <= _info.range) // 거리 확인
            {
                IsCompleted = true;
            }
        }
        else
        {
            _coolTime += Time.deltaTime;
        }
    }

    public override void Enter()
    {
        Debug.Log("내려찍기");
        originSize = _monster.ColliderSize;
        orginPoint = _monster.HitPoint.localPosition;
        _monster.HitPoint.localPosition = new Vector3(_hitPoint.x, _hitPoint.y, _hitPoint.z);
        _monster.ColliderSize = new Vector3(_hitScale.x, _hitScale.y, _hitScale.z);
        _monster.CharacterModel.localScale = new Vector3(-_monster.Direction, 1, 1);

        CoroutineRunner.Instance.StartCoroutine(MoveToPlayer());

        _monster.OnAttackAction += Attack;
        _monster.OnFinishSkill += Finish;
    }

    public override void Stay()
    {
        if (!_monster.Ani.GetBool("Takedown"))
        {
            _monster.Ani.SetBool("Takedown", true);
        }
    }

    public override void Exit()
    {
        _monster.Ani.SetBool("Takedown", false);
        _monster.ColliderSize = originSize;
        _monster.HitPoint.localPosition = orginPoint;
        _coolTime = 0;
        count = 0f;
        _isHit = false;

        IsCompleted = false;
    }

    IEnumerator MoveToPlayer()
    {
        yield return new WaitForSecondsRealtime(1.1f);

        _monster.Direction = _monster.Player.transform.position.x - _monster.transform.position.x;
        _monster.CharacterModel.localScale = new Vector3(-_monster.Direction, 1, 1);
        float dis = Vector3.Distance(_monster.transform.position, _monster.Player.transform.position - new Vector3(_monster.Direction, 0, 0));
        float firstDis = dis / 4;

        if (_monster.CharacterModel.transform.localScale.x < 0)
        {
            _monster.transform.DOMoveX(_monster.transform.position.x + firstDis, 0.6f);
        }
        else if (_monster.CharacterModel.transform.localScale.x > 0)
        {
            _monster.transform.DOMoveX(_monster.transform.position.x - firstDis, 0.6f);
        }

        yield return new WaitForSecondsRealtime(0.6f);

        _monster.Direction = _monster.Player.transform.position.x - _monster.transform.position.x;
        _monster.CharacterModel.localScale = new Vector3(-_monster.Direction, 1, 1);
        dis = Vector3.Distance(_monster.transform.position, _monster.Player.transform.position - new Vector3(_monster.Direction, 0, 0));
        float secondDis = dis / 4;

        if (_monster.CharacterModel.transform.localScale.x < 0)
        {
            _monster.transform.DOMoveX(_monster.transform.position.x + secondDis, 1f);
        }
        else if (_monster.CharacterModel.transform.localScale.x > 0)
        {
            _monster.transform.DOMoveX(_monster.transform.position.x - secondDis, 1f);
        }

        yield return new WaitForSecondsRealtime(1f);

        _monster.Direction = _monster.Player.transform.position.x - _monster.transform.position.x;
        _monster.CharacterModel.localScale = new Vector3(-_monster.Direction, 1, 1);
        dis = Vector3.Distance(_monster.transform.position, _monster.Player.transform.position - new Vector3(_monster.Direction, 0, 0));
        float thirdDis = dis;
        if (thirdDis > 5)
        {
            thirdDis = 5f;
        }

        TestSound.Instance.PlaySound("Takedown_sign");
        TestSound.Instance.PlaySound("Takedown_signVoice");
        if (_monster.CharacterModel.transform.localScale.x < 0)
        {
            _monster.transform.DOMoveX(_monster.transform.position.x + thirdDis, 1f);
        }
        else if (_monster.CharacterModel.transform.localScale.x > 0)
        {
            _monster.transform.DOMoveX(_monster.transform.position.x - thirdDis, 1f);
        }
    }

    private void Attack()
    {
        Collider[] hitPlayer = Physics.OverlapBox(_monster.HitPoint.position, _monster.ColliderSize / 2, _monster.HitPoint.rotation, _monster.PlayerLayer);

        if (count < _attackCount)
        {
            GameObject hitParticle = ObjectPool.Instance.Spawn("P_Slash", 1);
            TestSound.Instance.PlaySound("Takedown_1");
            TestSound.Instance.PlaySound("Takedown_1Voice");
            CameraController.Instance.SceneShaking();

            OnFlipEffect(hitParticle);
        }
        else
        {
            GameObject hitParticle = ObjectPool.Instance.Spawn("P_SlashCharge", 1);
            TestSound.Instance.PlaySound("Takedown_3");
            TestSound.Instance.PlaySound("Takedown_3Voice");
            CameraController.Instance.SceneShaking();

            OnFlipEffect(hitParticle);
        }

        foreach (Collider player in hitPlayer)
        {
            var _player = player.GetComponent<Player>();
            if (_player.IsInvincible) return;

            if (count < _attackCount)
            {
                Debug.Log("내려찍기 성공");
                _player.TakeDamage(_info.damage);
            }
            else
            {
                Debug.Log("내려찍기 피니쉬 성공");
                _player.TakeDamage(_finishDamage);
                _player.Knockback(GetKnockBackPosition(), _knockBackDuration);

                int dir = 1;
                if ((_monster.transform.position - _monster.Player.transform.position).x > 0)
                {
                    dir = -1;
                }

                _player.TakeStun(1f, dir);
                _isHit = true;
            }
        }

        if (_isHit)
        {
            //_monster.Ani.SetBool("Takedown_Groggy", true);
        }

        count++;
    }

    private Vector3 GetKnockBackPosition()
    {
        RaycastHit hit;
        Vector3 pos = _monster.transform.position;
        pos.y = 2.159f;

        if (Physics.Raycast(pos, Vector2.right * _monster.Direction, out hit, _knockBackPower * _knockBackDuration, _monster.WallLayer))
        {
            return hit.point;
        }

        Vector3 target = new Vector3(pos.x + ((Vector2.right * _monster.Direction) * (_knockBackPower * _knockBackDuration)).x, pos.y, pos.z);
        return target;
    }

    private void OnFlipEffect(GameObject obj)
    {
        if (_monster.CharacterModel.localScale.x > 0)
        {
            obj.transform.position = _monster.transform.position - new Vector3(1f, -2.6f);
        }
        else if (_monster.CharacterModel.localScale.x < 0)
        {
            obj.transform.position = _monster.transform.position - new Vector3(-1f, -2.6f);
        }

        obj.GetComponent<FlipSlash>().OnFlip(_monster.CharacterModel.localScale);
    }

    private void Finish()
    {
        _monster.FinishSkill();
    }
}
