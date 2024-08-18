using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Platform", menuName = "ScriptableObjects/EliteMonster/Skill/Platform", order = 1)]
public class Elite_Platform : Elite_Skill
{
    [SerializeField] private Elite_Skill _thunderStorke;
    [SerializeField] private Elite_Skill _explosion;

    private bool _isHitPointTempo;

    public override void Init(EliteMonster monster)
    {
        base.Init(monster);

        _thunderStorke.Init(monster);
        _explosion.Init(monster);

        _isHitPointTempo = false;
    }

    public override void Check()
    {
        _isCompleted = true;
    }

    public override void Enter()
    {
        _thunderStorke.Enter();

        _monster.Stat.OnPointTempo += () =>
        {
            if (!_isHitPointTempo)
            {
                CoroutineRunner.Instance.StartCoroutine(StartExplosion());
            }
        };
    }
    public override void Stay()
    {
        
        if (_isHitPointTempo)
        {
            _explosion.Stay();
        }

        _thunderStorke.Stay();


    }

    public override void Exit()
    {
        _thunderStorke.Exit();
        _explosion.Exit();

        _isHitPointTempo = false;
        _monster.Stat.OnPointTempo = null;

        _monster.ResetSkill();
        _isCompleted = false;
    }

    private IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(1f);

        _isHitPointTempo = true;
        _explosion.Enter();
    }

  
}