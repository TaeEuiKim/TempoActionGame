using UnityEngine;

[CreateAssetMenu(fileName = "Thunderstroke", menuName = "ScriptableObjects/EliteMonster/Skill/Thunderstroke", order = 1)]
public class Elite_Thunderstroke : Elite_Skill
{
    [SerializeField] private float _executeDuration;
    private float _executeTime;

    [SerializeField] private float _lightningCount;

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _executeTime = 0;
    }

    public override void Check()
    {
        _isCompleted = true;
    }

    public override void Enter()
    {
        Debug.Log("³«·Ú");
        _monster.Stat.OnPointTempo += () =>
        {
            _monster.CurrentSkill = null;
        };
    }
    public override void Stay()
    {
        if (_executeTime >= _executeDuration)
        {
            for (int i = 0; i < _lightningCount; i++)
            {
                ExecuteLightning();
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
        _monster.ResetSkill();
        //_isCompleted = false;
    }

    // ³«·Ú »ý¼º ÇÔ¼ö
    private void ExecuteLightning()
    {
        int randomIndex = Random.Range(0, _monster.CreatePlatform.CurPlatformList.Count);
        Vector3 executePosition = _monster.CreatePlatform.CurPlatformList[randomIndex].transform.position;

        GameObject lightning = ObjectPool.Instance.Spawn("Lightning", 2);
        lightning.transform.position = executePosition;

        lightning.GetComponent<Lightning>().totalDamage = _monster.Stat.AttackDamage * (_info.damage / 100);
    }
}