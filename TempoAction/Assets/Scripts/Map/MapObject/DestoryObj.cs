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

    [Space]
    [Header("플레이어 킬 카운트 조건 여부")]
    [SerializeField] private bool isKill = false;
    [Header("플레이어가 몇 킬을 해야 무적이 풀릴건지")]
    [SerializeField] private int killCount = 0;

    private Player _player;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    protected override void Awake()
    {
        base.Awake();
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (isKill)
        {
            _player = FindObjectOfType<Player>();
            if (isDestory)
            {
                isDestory = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (isKill)
        {
            if (killCount <= _player.killCount)
            {
                isDestory = true;
            }
        }
    }

    private void DestroyObject()
    {
        ObjectPool.Instance.Remove(this.gameObject);
    }

    public override void TakeDamage(float value)
    {
        base.TakeDamage(value);
        CheckRenderObject();

        if (Hp <= 0)
        {
            switch (_foodType)
            {
                case Define.FoodType.BANANA:
                    ObjectPool.Instance.Spawn("Banana").transform.position = transform.position + new Vector3(0, 0.5f);
                    break;
                case Define.FoodType.CAKE:
                    ObjectPool.Instance.Spawn("Cake").transform.position = transform.position + new Vector3(0, 0.5f);
                    break;
                case Define.FoodType.COKE:
                    ObjectPool.Instance.Spawn("Coke").transform.position = transform.position + new Vector3(0, 0.5f);
                    break;
                case Define.FoodType.DUMPLINGS:
                    ObjectPool.Instance.Spawn("Dumplings").transform.position = transform.position + new Vector3(0, 0.5f);
                    break;
                case Define.FoodType.HAMBURGER:
                    ObjectPool.Instance.Spawn("Hamburger").transform.position = transform.position + new Vector3(0, 0.5f);
                    break;
                case Define.FoodType.HOTDOG:
                    ObjectPool.Instance.Spawn("Hotdog").transform.position = transform.position + new Vector3(0, 0.5f);
                    break;
                case Define.FoodType.NONE:
                    break;
            }
        }
    }

    private void CheckRenderObject()
    {
        switch (_type)
        {
            case Define.DestoryObjectType.FENCE:
                SoundManager.Instance.PlayOneShot("event:/SFX_BG_OBJ_BarbedWire", transform);

                if (Hp <= 0)
                {
                    _skinnedMeshRenderer.SetBlendShapeWeight(0, 100);
                    _skinnedMeshRenderer.SetBlendShapeWeight(1, 100);
                    Ani.SetBool("Destory", true);
                    GetComponent<BoxCollider>().enabled = false;
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
                SoundManager.Instance.PlayOneShot("event:/SFX_BG_OBJ_Garbage_1", transform);

                if (Hp <= 0)
                {
                    _skinnedMeshRenderer.SetBlendShapeWeight(Random.Range(0, 2), 100);
                    Ani.SetBool("Destory", true);
                    GetComponent<SphereCollider>().enabled = false;
                }
                else if (Hp <= _hpCounts[0])
                {
                    _skinnedMeshRenderer.SetBlendShapeWeight(Random.Range(0, 2), 100);
                }
                break;
            case Define.DestoryObjectType.BOX:
                if (Hp <= 0)
                {
                    Ani.SetBool("Destory", true);
                    GetComponent<BoxCollider>().enabled = false;
                }
                break;
            case Define.DestoryObjectType.FLOOR:
                Animator[] anis = GetComponentsInChildren<Animator>();

                SoundManager.Instance.PlayOneShot("event:/SFX_BG_OBJ_WoodBreak", transform);
                SoundManager.Instance.PlayOneShot("event:/SFX_BG_OBJ_WoodWreck", transform);

                if (anis.Length > 0)
                {
                    for (int i = 0; i < anis.Length; ++i)
                    {
                        anis[i].SetBool("Destory", true);
                    }
                }
                GetComponent<BoxCollider>().enabled = false;
                break;
            default:
                break;
        }
    }
}
