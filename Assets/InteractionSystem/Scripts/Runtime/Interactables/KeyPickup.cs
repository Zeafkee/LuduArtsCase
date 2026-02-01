using LuduInteraction.Runtime.Core;
using LuduInteraction.Runtime.Player;
using UnityEngine;

namespace LuduInteraction.Runtime.Interactables
{
    /// <summary>
    /// An interactable item that can be picked up and added to the player's inventory.
    /// </summary>
    public class KeyPickup : InstantInteractable
    {
        #region Fields

        [Header("Key Settings")]
        [Tooltip("The item data to add to inventory.")]
        [SerializeField] private ItemData m_KeyItem;

        #endregion

        #region Properties

        public override string InteractionPrompt => $"Pick up {m_KeyItem?.ItemName ?? "Key"}";
        public override bool CanInteract => true;

        #endregion

        #region Unity Methods

        protected override void Start()
        {
            base.Start(); // Load ID logic

            // Check if already collected
            if (m_KeyItem != null && SimpleInventory.Instance != null)
            {
                if (SimpleInventory.Instance.HasItem(m_KeyItem.ItemID))
                {
                    gameObject.SetActive(false);
                }
            }
        }

        #endregion

        #region Overrides

        public override void OnInteractComplete(GameObject interactor)
        {
            if (m_KeyItem != null && SimpleInventory.Instance != null)
            {
                SimpleInventory.Instance.AddItem(m_KeyItem);
                
                // Disable the object to simulate "picking it up"
                gameObject.SetActive(false);
            }
            else if (SimpleInventory.Instance == null)
            {
                Debug.LogWarning("SimpleInventory Instance is null!");
            }
        }

        #endregion
    }
}
