using UnityEngine;

namespace LuduInteraction.Runtime.Core
{
    /// <summary>
    /// Base class for objects that toggle between two states (e.g., Open/Closed).
    /// </summary>
    public abstract class ToggleInteractable : BaseInteractable
    {
        #region Fields

        [Tooltip("Current toggle state.")]
        [SerializeField] protected bool m_IsOn;

        #endregion

        #region Properties

        /// <inheritdoc />
        public override InteractionType InteractionType => InteractionType.Toggle;

        /// <summary>
        /// Gets the current state.
        /// </summary>
        public bool IsOn => m_IsOn;

        #endregion

        #region IInteractable Implementation

        /// <inheritdoc />
        public override void OnInteractStart(GameObject interactor)
        {
            base.OnInteractStart(interactor);
            
            // Toggle state logic
            m_IsOn = !m_IsOn;
            
            OnInteractComplete(interactor);
            
            // Save state
            Save();
        }

        #endregion

        #region ISaveable Implementation

        public override void LoadState(bool state)
        {
            m_IsOn = state;
            // Visuals should be updated by concrete classes (Door/Switch) in their Awake/Start based on m_IsOn
        }

        public override bool SaveState()
        {
            return m_IsOn;
        }

        #endregion
    }
}
