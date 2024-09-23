using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGenerator : MonoBehaviour
{
    public GameObject prefab;

    void Start()
    {
        var obj = Instantiate(prefab);

        NormalSkill sword = new NormalSkill(500, 51, "¹ßµµ¼ú", Define.SkillColliderType.FORWARD, 150, 250, Define.SkillEffectType.RUSH, 800);

        var so = obj.GetComponent<SkillObject>();
        so.Initialize(sword);
    }
}
