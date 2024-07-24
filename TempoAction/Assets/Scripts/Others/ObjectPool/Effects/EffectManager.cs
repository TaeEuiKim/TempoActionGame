using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    private ObjectPool _pool;
    public ObjectPool Pool{ get=>_pool; }

    [Header("À§Ä¡")]
    public Transform rightSparkPoint;
    public Transform leftSparkPoint;

    private void Start()
    {
        _pool = GetComponent<ObjectPool>();
    }
}
