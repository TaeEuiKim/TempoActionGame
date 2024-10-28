public class Normal_AttackState : Normal_State
{
    public Normal_AttackState(NormalMonster monster) : base(monster)
    {
    }

    public override void Stay()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
        _monster.Ani?.SetBool("Attack", false);
    }
}
