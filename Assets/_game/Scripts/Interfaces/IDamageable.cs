public interface IDamageable
{
    public bool IsDead { get; }

    public void TakeDamage(float damage);
}