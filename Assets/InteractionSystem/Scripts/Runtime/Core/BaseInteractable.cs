using System.Collections.Generic;
using UnityEngine;

namespace LuduInteraction.Runtime.Core
{
    /// <summary>
    /// Base abstract class for all interactable objects.
    /// Implements common functionality for IInteractable.
    /// </summary>
    public abstract class BaseInteractable : MonoBehaviour, IInteractable, ISaveable
    {
        #region Fields

        [Tooltip("Unique ID for saving state.")]
        [SerializeField] private string m_SaveID;

        [Tooltip("Text to display when the player looks at this object.")]
        [SerializeField] private string m_InteractionPrompt = "Interact";

        [Tooltip("Whether the object is currently interactable.")]
        [SerializeField] private bool m_IsInteractable = true;

        [Header("Audio")]
        [SerializeField] protected AudioSource m_AudioSource;
        [SerializeField] protected AudioClip m_InteractionSound;

        [Header("Visuals")]
        [Tooltip("Material to apply for outlining when focused.")]
        [SerializeField] private Material m_OutlineMaterial;

        #endregion

        #region Properties

        public string SaveID => m_SaveID;

        /// <inheritdoc />
        public virtual string InteractionPrompt => m_InteractionPrompt;

        /// <inheritdoc />
        public virtual bool CanInteract => m_IsInteractable;

        /// <inheritdoc />
        public abstract InteractionType InteractionType { get; }

        #endregion

        #region Unity Methods

        protected virtual void Start()
        {
            if (!string.IsNullOrEmpty(m_SaveID) && SaveManager.Instance != null)
            {
                Debug.Log($"[{gameObject.name}] Loading state for ID: {m_SaveID}");
                bool savedState = SaveManager.Instance.GetBool(m_SaveID);
                LoadState(savedState);
            }
            else if (string.IsNullOrEmpty(m_SaveID))
            {
                Debug.LogWarning($"[{gameObject.name}] SaveID is empty! State will not be saved.");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates a new GUID for this object.
        /// </summary>
        public void GenerateSaveID()
        {
            m_SaveID = System.Guid.NewGuid().ToString();
        }

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

        /// <summary>
        /// Toggles the outline material on all child renderers.
        /// </summary>
        public void SetHighlight(bool active)
        {
            if (m_OutlineMaterial == null) return;

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (var rend in renderers)
            {
                var mats = new System.Collections.Generic.List<Material>(rend.sharedMaterials);
                
                if (active)
                {
                    // Add if not present
                    if (!mats.Contains(m_OutlineMaterial))
                    {
                        mats.Add(m_OutlineMaterial);
                    }
                }
                else
                {
                    // Remove if present
                    mats.Remove(m_OutlineMaterial);
                }

                rend.materials = mats.ToArray();
            }
        }

        #endregion

        #region IInteractable Implementation

        /// <inheritdoc />
        public virtual void OnInteractStart(GameObject interactor)
        {
            Debug.Log($"{interactor.name} started interacting with {gameObject.name}");
        }

        /// <inheritdoc />
        public virtual void OnInteractHold(GameObject interactor, float progress)
        {
            // By default, do nothing for hold unless overridden
        }

        /// <inheritdoc />
        public abstract void OnInteractComplete(GameObject interactor);

        /// <inheritdoc />
        public virtual void OnInteractCancel(GameObject interactor)
        {
            Debug.Log($"{interactor.name} canceled interaction with {gameObject.name}");
        }

        #endregion

        #region ISaveable Implementation

        public virtual void LoadState(bool state)
        {
            // Override in subclasses
        }

        public virtual bool SaveState()
        {
            // Override in subclasses to return current state
            return false;
        }

        protected void Save()
        {
            if (!string.IsNullOrEmpty(m_SaveID) && SaveManager.Instance != null)
            {
                SaveManager.Instance.SetState(m_SaveID, SaveState());
            }
        }

        #endregion
    }
}
