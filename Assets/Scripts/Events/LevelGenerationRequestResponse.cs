namespace GudKoodi.DeeperSkeeper.Event
{
    using System;
    using UnityEngine.Events;

    /// <summary>
    /// Mandatory Unity and linting overhead.
    /// </summary>
    [Serializable]
    public class LevelGenerationRequestResponse : UnityEvent<int>
    {
    }
}
