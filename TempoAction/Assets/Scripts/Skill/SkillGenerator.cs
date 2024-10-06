using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGenerator : MonoBehaviour
{
    public GameObject prefab;
    public SkillRunnerBase skillData;

    void Start()
    {
        var obj = Instantiate(prefab);

        NormalSkill sword = new NormalSkill(skillData);

        var so = obj.GetComponent<SkillObject>();
        so.Initialize(sword);
    }
}
