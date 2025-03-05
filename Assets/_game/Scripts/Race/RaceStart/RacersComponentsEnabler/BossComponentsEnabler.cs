public class BossComponentsEnabler : EnemyComponentsEnabler
{
    protected override bool IsBoss
    {
        get { return true; }
    }
}