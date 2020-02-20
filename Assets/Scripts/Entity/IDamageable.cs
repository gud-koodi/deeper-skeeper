namespace GudKoodi.DeeperSkeeper.Entity
{
    /// <summary>
    /// Can be damaged.
    /// </summary>
    public interface IDamageable
    {
        void ApplyDamage(float damage);
    }
}