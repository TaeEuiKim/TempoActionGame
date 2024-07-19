using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticle : PoolObject
{


    private void Update()
    {
        if (!transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {
            pool.ReturnObject(gameObject);
        }
    }
}
