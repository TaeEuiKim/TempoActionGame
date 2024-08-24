using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunderstroke", menuName = "ScriptableObjects/EliteMonster/Skill/Thunderstroke", order = 1)]
public class Elite_Thunderstroke : Elite_Skill
{

    [SerializeField] private float _executeDuration;
    private float _executeTime;

    [SerializeField] private float _lightningCount;

    private List<BuffPlatform> tempPlatforms;
    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _executeTime = 0;

    }

    public override bool Check()
    {

        return false;

    }

    public override void Enter()
    {
        Debug.Log("³«·Ú");
        _monster.OnPointTempo += () =>
        {
            _monster.FinishSkill();
        };
    }
    public override void Stay()
    {
        if (_executeTime >= _executeDuration)
        {
            
            tempPlatforms = _monster.CreatePlatform.CurPlatformList.ToList();

            for (int i = 0; i < _lightningCount; i++)
            {
                int randomIndex = Random.Range(0, tempPlatforms.Count);
                Vector3 executePosition = tempPlatforms[randomIndex].transform.position;
                tempPlatforms.RemoveAt(randomIndex);

                CoroutineRunner.Instance.StartCoroutine(ExecuteLightning(executePosition));
            }

            
            _executeTime = 0;
        }
        else
        {
            _executeTime += Time.deltaTime;
        }
    }
    public override void Exit()
    {
        _executeTime = 0;
    }

    // ³«·Ú »ý¼º ÇÔ¼ö
    private IEnumerator ExecuteLightning(Vector3 point)
    {
        GameObject lightningReady = ObjectPool.Instance.Spawn("FX_LightningReady", 1f);
        lightningReady.transform.position = point;

        yield return new WaitForSeconds(0.5f);

        GameObject lightning = ObjectPool.Instance.Spawn("Lightning", 2f);
        lightning.transform.position = point + new Vector3(0, 0.2f, 0);
        lightning.GetComponent<Lightning>().TotalDamage = _monster.Stat.Damage * (_info.damage / 100);

        lightning.GetComponent<Collider>().enabled = true;

        yield return new WaitForSeconds(0.5f);
        lightning.GetComponent<Collider>().enabled = false;

    }
}