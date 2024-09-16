using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dropbomb", menuName = "ScriptableObjects/MiddleMonster/Skill/Dropbomb", order = 1)]
public class Middle_Dropbomb : Middle_Skill
{
    [SerializeField] private float dropTime;        // ∆¯≈∫ ≈ı«œ ¡÷±‚
    [SerializeField] private float dropEndTime;     // ∆¯≈∫ ≈ı«œ Ω√∞£
    [SerializeField] private int bombAmount;       // ∆¯≈∫ ∞≥ºˆ

    private float _startTime = 0f;
    private float _dropTimer = 0f;
    private int _bombCount = 0;
    private bool isFlip = false;

    private float _coolTime = 0f;

    public override void Init(MiddleMonster monster)
    {
        base.Init(monster);

        isFlip = false;
        _startTime = 0f;
        _dropTimer = 10f;
        _bombCount = 0;
    }

    public override void Check()
    {
        if (IsCompleted) return;

        if (_coolTime >= _info.coolTime) // ƒ≈∏¿” »Æ¿Œ
        {
            IsCompleted = true;
        }
        else
        {
            _coolTime += Time.deltaTime;
        }
    }

    public override void Enter()
    {
        Debug.Log("∆¯≈∫ ≈ı«œ");

        _monster.transform.DOMoveX(_monster.middlePoint[Define.MiddleMonsterPoint.BOMBLEFTPOINT].position.x, dropEndTime);
}
    public override void Stay()
    {
        //if (!_monster.Ani.GetBool("Dropbomb"))
        //{
        //    _monster.Ani.SetBool("Dropbomb", true);
        //}

        if (_startTime < dropEndTime && !isFlip)
        {
            if (_dropTimer > dropTime && bombAmount > _bombCount)
            {
                CreateBomb();
                _bombCount++;
                _dropTimer = 0f;
            }

            _dropTimer += Time.deltaTime;
            _startTime += Time.deltaTime;

            return;
        }

        if (!isFlip && _startTime >= dropEndTime)
        {
            _startTime = 0f;
            _dropTimer = 10f;
            _bombCount = 0;
            _monster.transform.DOMoveX(_monster.middlePoint[Define.MiddleMonsterPoint.BOMBRIGHTPOINT].position.x, dropEndTime);
            isFlip = true;
        }

        if (_startTime < dropEndTime && isFlip)
        {
            if (_dropTimer > dropTime && bombAmount > _bombCount)
            {
                CreateBomb();
                _bombCount++;
                _dropTimer = 0f;
            }

            _dropTimer += Time.deltaTime;
            _startTime += Time.deltaTime;
        }
    }

    public override void Exit()
    {

    }

    private void CreateBomb()
    {
        GameObject bomb = ObjectPool.Instance.Spawn("Bomb");
        bomb.transform.position = _monster.HitPoint.position;

        if (!isFlip)
        {
            bomb.GetComponent<Rigidbody>().AddForce(Vector3.left * 1.2f, ForceMode.Impulse);
        }
        else
        {
            bomb.GetComponent<Rigidbody>().AddForce(Vector3.right * 1.2f, ForceMode.Impulse);
        }
    }
}
