using UnityEngine;

namespace LuduInteraction.Runtime.Core
{
    /// <summary>
    /// Base class for objects that interact immediately upon input.
    /// </summary>
    public abstract class InstantInteractable : BaseInteractable
    {
        #region Properties

        /// <inheritdoc />
        public override InteractionType InteractionType => InteractionType.Instant;

        #endregion

        #region IInteractable Implementation

        /// <inheritdoc />
        public override void OnInteractStart(GameObject interactor)
        {
            base.OnInteractStart(interactor);
            
            // For instant interactions, start implies complete
            OnInteractComplete(interactor);
        }

        #endregion
    }
}
