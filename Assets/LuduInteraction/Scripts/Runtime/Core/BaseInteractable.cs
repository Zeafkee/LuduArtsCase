using UnityEngine;

namespace LuduInteraction.Runtime.Core
{
    /// <summary>
    /// Base abstract class for all interactable objects.
    /// Implements common functionality for IInteractable.
    /// </summary>
    public abstract class BaseInteractable : MonoBehaviour, IInteractable
    {
        #region Fields

        [Tooltip("Text to display when the player looks at this object.")]
        [SerializeField] private string m_InteractionPrompt = "Interact";

        [Tooltip("Whether the object is currently interactable.")]
        [SerializeField] private bool m_IsInteractable = true;

        [Header("Audio")]
        [SerializeField] protected AudioSource m_AudioSource;
        [SerializeField] protected AudioClip m_InteractionSound;

        #endregion

        #region Properties

        /// <inheritdoc />
        public virtual string InteractionPrompt => m_InteractionPrompt;

        /// <inheritdoc />
        public virtual bool CanInteract => m_IsInteractable;

        /// <inheritdoc />
        public abstract InteractionType InteractionType { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the interaction prompt text dynamically.
        /// </summary>
        /// <param name="prompt">New prompt text.</param>
        public void SetInteractionPrompt(string prompt)
        {
            m_InteractionPrompt = prompt;
        }

        /// <summary>
        /// Enables or disables interaction.
        /// </summary>
        /// <param name="state">True to enable, false to disable.</param>
        public void SetInteractableState(bool state)
        {
            m_IsInteractable = state;
        }

        #endregion

        #region IInteractable Implementation

        /// <inheritdoc />
        public virtual void OnInteractStart(GameObject interactor)
        {
            Debug.Log($"{interactor.name} started interacting with {gameObject.name}");
            // Removed: SetInteractableState(false); 
        }

        /// <inheritdoc />
        public virtual void OnInteractHold(GameObject interactor, float progress)
        {
            // By default, do nothing for hold unless overridden
        }

        /// <inheritdoc />
        public virtual void OnInteractComplete(GameObject interactor)
        {
            Debug.Log($"{interactor.name} completed interaction with {gameObject.name}");
            // Stay interactable so we can toggle or interact again
        }

        /// <inheritdoc />
        public virtual void OnInteractCancel(GameObject interactor)
        {
            Debug.Log($"{interactor.name} canceled interaction with {gameObject.name}");
        }

        #endregion
    }
}
