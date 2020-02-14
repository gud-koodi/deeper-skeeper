namespace GudKoodi.DeeperSkeeper.Event
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Mandatory Unity and linting overhead.
    /// </summary>
    [Serializable]
    public class ObjectCreationResponse : UnityEvent<GameObject>
    {
    }
}
