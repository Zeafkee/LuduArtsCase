using DG.Tweening;
using LuduInteraction.Runtime.Core;
using LuduInteraction.Runtime.Player;
using UnityEngine;

namespace LuduInteraction.Runtime.Interactables
{
    /// <summary>
    /// A chest that requires holding to open and adds an item to the player's inventory.
    /// </summary>
    public class Chest : HoldInteractable
    {
        #region Fields

        [Header("Chest Settings")]
        [Tooltip("The item inside the chest (Optional).")]
        [SerializeField] private ItemData m_StoredItem;
        
        [Tooltip("If true, the chest is already open.")]
        [SerializeField] private bool m_IsOpen;

        [Header("Animation Settings")]
        [Tooltip("The Transform of the lid to rotate.")]
        [SerializeField] private Transform m_LidTransform;
        
        [Tooltip("Rotation when fully opened.")]
        [SerializeField] private Vector3 m_OpenedRotationEuler = new Vector3(-90, 0, 0);
        
        [SerializeField] private float m_OpenDuration = 1.0f;
        [SerializeField] private Ease m_OpenEase = Ease.OutBounce;

        [Header("Prompts")]
        [SerializeField] private string m_ClosedPrompt = "Hold to Open";
        [SerializeField] private string m_EmptyPrompt = "Empty";

        #endregion

        #region Properties

        public override string InteractionPrompt => m_IsOpen ? m_EmptyPrompt : m_ClosedPrompt;
        
        // Disable interaction if already open
        public override bool CanInteract => base.CanInteract && !m_IsOpen;

        #endregion

        #region Overrides

        public override void OnInteractComplete(GameObject interactor)
        {
            if (m_IsOpen) return;

            OpenChest(interactor);
        }

        #endregion

        #region ISaveable Implementation

        public override void LoadState(bool state)
        {
            m_IsOpen = state;
            if (m_IsOpen && m_LidTransform != null)
            {
                // Snap to open
                m_LidTransform.localRotation = Quaternion.Euler(m_OpenedRotationEuler);
            }
        }

        public override bool SaveState()
        {
            return m_IsOpen;
        }

        #endregion

        #region Private Methods

        private void OpenChest(GameObject interactor)
        {
            m_IsOpen = true;
            Save(); // Save state

            Debug.Log("Chest Opened!");

            // 1. Animation (DOTween)
            if (m_LidTransform != null)
            {
                // Convert Euler to Quaternion for consistency with your request
                Quaternion targetRot = Quaternion.Euler(m_OpenedRotationEuler);
                m_LidTransform.DOLocalRotateQuaternion(targetRot, m_OpenDuration).SetEase(m_OpenEase);
            }

            // Play Sound
            if (m_AudioSource != null && m_InteractionSound != null)
            {
                m_AudioSource.PlayOneShot(m_InteractionSound);
            }

            // 2. Give Item
            if (m_StoredItem != null)
            {
                if (SimpleInventory.Instance != null)
                {
                    SimpleInventory.Instance.AddItem(m_StoredItem);
                }
                else
                {
                    Debug.LogWarning("SimpleInventory Instance is null.");
                }
            }

            // 3. Disable future interaction
            // We keep "CanInteract" logic in the property override, 
            // but we can also explicitly set the prompt.
        }

        #endregion
    }
}
