namespace GudKoodi.DeeperSkeeper.Event
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Mandatory Unity overhead.
    /// </summary>
    [Serializable]
    public class ObjectUpdateRequestResponse : UnityEvent<GameObject>
    {
    }
}
