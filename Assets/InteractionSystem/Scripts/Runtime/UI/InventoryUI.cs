using System.Collections.Generic;
using LuduInteraction.Runtime.Core;
using LuduInteraction.Runtime.Player;
using UnityEngine;

namespace LuduInteraction.Runtime.UI
{
    /// <summary>
    /// Manages a fixed grid of inventory slots.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [Tooltip("Reference to the player's inventory.")]
        [SerializeField] private SimpleInventory m_Inventory;

        [Tooltip("The fixed list of slots in the UI (e.g., 6 slots).")]
        [SerializeField] private List<InventorySlot> m_Slots = new List<InventorySlot>();

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (m_Inventory == null)
            {
                m_Inventory = SimpleInventory.Instance;
            }

            if (m_Inventory != null)
            {
                m_Inventory.OnInventoryChanged.AddListener(UpdateUI);
                UpdateUI();
            }
        }

        private void Start()
        {
            // Retry logic in case OnEnable ran before Inventory was ready
            if (m_Inventory == null)
            {
                m_Inventory = SimpleInventory.Instance;
                if (m_Inventory != null)
                {
                    m_Inventory.OnInventoryChanged.AddListener(UpdateUI);
                    UpdateUI();
                }
            }
            else
            {
                // Force an update in Start just to be sure
                UpdateUI();
            }
        }

        private void OnDisable()
        {
            if (m_Inventory != null)
            {
                m_Inventory.OnInventoryChanged.RemoveListener(UpdateUI);
            }
        }

        #endregion

        #region Private Methods

        private void UpdateUI()
        {
            if (m_Inventory == null || m_Slots.Count == 0) return;

            List<ItemData> items = m_Inventory.GetItems();
            Debug.Log($"[InventoryUI] Updating UI with {items.Count} items.");

            // Loop through all UI slots
            for (int i = 0; i < m_Slots.Count; i++)
            {
                if (i < items.Count)
                {
                    // Assign the item from the inventory list to the UI slot
                    m_Slots[i].AssignItem(items[i]);
                }
                else
                {
                    // If no item exists for this index, clear the slot
                    m_Slots[i].ClearSlot();
                }
            }
        }

        #endregion
    }
}
