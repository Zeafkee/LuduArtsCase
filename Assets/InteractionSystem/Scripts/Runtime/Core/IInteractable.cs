using UnityEngine;

namespace LuduInteraction.Runtime.Core
{
    /// <summary>
    /// Defines the contract for any object that can be interacted with.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// The text to display in the UI (e.g., "Open Chest").
        /// </summary>
        string InteractionPrompt { get; }

        /// <summary>
        /// Can the object currently be interacted with?
        /// </summary>  
        bool CanInteract { get; }

        /// <summary>
        /// The type of interaction (Instant, Hold, Toggle).
        /// </summary>
        InteractionType InteractionType { get; }

        /// <summary>
        /// Called when the interaction key is pressed down.
        /// </summary>
        void OnInteractStart(GameObject interactor);

        /// <summary>
        /// Called every frame while holding (for Hold interactions).
        /// </summary>
        /// <param name="progress">0.0 to 1.0 normalized progress.</param>
        void OnInteractHold(GameObject interactor, float progress);

        /// <summary>
        /// Called when the interaction is successfully finished (or button released for Instant).
        /// </summary>
        void OnInteractComplete(GameObject interactor);

        /// <summary>
        /// Called if the player releases the key before the Hold is complete.
        /// </summary>
        void OnInteractCancel(GameObject interactor);
    }
}
