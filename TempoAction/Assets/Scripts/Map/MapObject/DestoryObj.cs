using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryObj : BaseObject
{
    [Header("찌그러짐 정도에 도달하는 체력")]
    [SerializeField] private float[] _hpCounts;
    [Header("오브젝트 타입")]
    [SerializeField] private Define.DestoryObjectType _type;
    [Header("부쉈을 때 나오는 음식")]
    [SerializeField] private Define.FoodType _foodType;

    private SkinnedMeshRenderer _skinnedMeshRenderer;

    protected override void Awake()
    {
        base.Awake();
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    private void DestroyObject()
    {
        ObjectPool.Instance.Remove(this.gameObject);
    }

    public override void TakeDamage(float value)
    {
        base.TakeDamage(value);
        CheckRenderObject();

        if (Hp <= 0 && _type == Define.DestoryObjectType.BOX)
        {
            switch (_foodType)
            {
                case Define.FoodType.BANANA:
                    ObjectPool.Instance.Spawn("Banana").transform.position = transform.position;
                    break;
                case Define.FoodType.CAKE:
                    ObjectPool.Instance.Spawn("Cake").transform.position = transform.position;
                    break;
                case Define.FoodType.COKE:
                    ObjectPool.Instance.Spawn("Coke").transform.position = transform.position;
                    break;
                case Define.FoodType.DUMPLINGS:
                    ObjectPool.Instance.Spawn("Dumplings").transform.position = transform.position;
                    break;
                case Define.FoodType.HAMBURGER:
                    ObjectPool.Instance.Spawn("Hamburger").transform.position = transform.position;
                    break;
                case Define.FoodType.HOTDOG:
                    ObjectPool.Instance.Spawn("Hotdog").transform.position = transform.position;
                    break;
            }
        }
    }

    private void CheckRenderObject()
    {
        switch (_type)
        {
            case Define.DestoryObjectType.FENCE:
                if (Hp <= 0)
                {
                    _skinnedMeshRenderer.SetBlendShapeWeight(0, 100);
                    _skinnedMeshRenderer.SetBlendShapeWeight(1, 100);
                    Ani.SetBool("Destory", true);
                }
                else if (Hp <= _hpCounts[1])
                {
                    _skinnedMeshRenderer.SetBlendShapeWeight(0, 100);
                    _skinnedMeshRenderer.SetBlendShapeWeight(1, 100);
                }
                else if (Hp <= _hpCounts[0])
                {
                    _skinnedMeshRenderer.SetBlendShapeWeight(0, 100);
                }
                break;
            case Define.DestoryObjectType.TRASH:
                if (Hp <= 0)
                {
                    _skinnedMeshRenderer.SetBlendShapeWeight(Random.Range(0, 2), 100);
                    Ani.SetBool("Destory", true);
                }
                else if (Hp <= _hpCounts[0])
                {
                    _skinnedMeshRenderer.SetBlendShapeWeight(Random.Range(0, 2), 100);
                }
                break;
            default:
                break;
        }
    }
}
