namespace GudKoodi.DeeperSkeeper.Entity
{
    /// <summary>
    /// Can be damaged.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Applies damage.
        /// </summary>
        /// <param name="damage"></param>
        void ApplyDamage(float damage);
    }
}