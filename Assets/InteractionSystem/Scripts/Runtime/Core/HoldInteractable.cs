using UnityEngine;

namespace LuduInteraction.Runtime.Core
{
    /// <summary>
    /// Base class for objects that require holding the interaction input.
    /// </summary>
    public abstract class HoldInteractable : BaseInteractable
    {
        #region Fields

        [Tooltip("Time in seconds required to hold for interaction to complete.")]
        [SerializeField] private float m_HoldDuration = 2.0f;

        #endregion

        #region Properties

        /// <inheritdoc />
        public override InteractionType InteractionType => InteractionType.Hold;

        /// <summary>
        /// Gets the required hold duration.
        /// </summary>
        public float HoldDuration => m_HoldDuration;

        #endregion
    }
}
