/* 
    GameEvent pattern using ScriptableObject as presented by Ryan Hipple.
    More info at: https://github.com/roboryantron/Unite2017

    Extended to support maximum amount of unity event's arguments.
 */

namespace GudKoodi.DeeperSkeeper.Event
{
    /// <summary>
    /// Interface for all event listeners.
    /// </summary>
    /// <typeparam name="T0">Type of first argument.</typeparam>
    /// <typeparam name="T1">Type of second argument.</typeparam>
    /// <typeparam name="T2">Type of third argument.</typeparam>
    /// <typeparam name="T3">Type of fourth argument.</typeparam>
    public interface IListener<T0, T1, T2, T3>
    {
        /// <summary>
        /// Invokes response on trigger.
        /// </summary>
        /// <param name="p0">First parameter.</param>
        /// <param name="p1">Second parameter.</param>
        /// <param name="p2">Third parameter.</param>
        /// <param name="p3">Fourth parameter.</param>
        void OnTriggered(T0 p0, T1 p1, T2 p2, T3 p3);
    }
}
