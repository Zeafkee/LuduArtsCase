using System.Collections.Generic;
using LuduInteraction.Runtime.Core;
using UnityEngine;
using UnityEngine.Events;

namespace LuduInteraction.Runtime.Player
{
    /// <summary>
    /// A simple inventory system for the player to track collected items.
    /// </summary>
    public class SimpleInventory : MonoBehaviour
    {
        #region Fields

        public static SimpleInventory Instance { get; private set; }

        [Tooltip("List of items currently in the inventory.")]
        [SerializeField] private List<ItemData> m_Items = new List<ItemData>();

        [Header("Save Settings")]
        [SerializeField] private string m_SaveID = "PlayerInventory";

        #endregion

        #region Events

        /// <summary>
        /// Invoked whenever an item is added or removed.
        /// </summary>
        public UnityEvent OnInventoryChanged;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                LoadInventory(); // Load immediately so data is ready for others in Start()
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an item to the inventory.
        /// </summary>
        public void AddItem(ItemData item)
        {
            if (item == null)
            {
                Debug.LogWarning("Attempted to add a null item to inventory.");
                return;
            }

            m_Items.Add(item);
            Debug.Log($"Collected: {item.ItemName}");
            OnInventoryChanged?.Invoke();
            
            SaveInventory();
        }

        /// <summary>
        /// Checks if the inventory contains a specific item by reference.
        /// </summary>
        public bool HasItem(ItemData item)
        {
            return m_Items.Contains(item);
        }

        /// <summary>
        /// Checks if the inventory contains an item with a specific ID.
        /// </summary>
        public bool HasItem(string itemID)
        {
            return m_Items.Exists(i => i.ItemID == itemID);
        }

        /// <summary>
        /// Returns a read-only list of items.
        /// </summary>
        public List<ItemData> GetItems()
        {
            return new List<ItemData>(m_Items);
        }

        #endregion

        #region Save/Load

        private void SaveInventory()
        {
            if (LuduInteraction.Runtime.Core.SaveManager.Instance == null) return;

            InventorySaveData data = new InventorySaveData();
            foreach (var item in m_Items)
            {
                if (item != null) data.ItemIDs.Add(item.ItemID);
            }

            string json = JsonUtility.ToJson(data);
            LuduInteraction.Runtime.Core.SaveManager.Instance.SetState(m_SaveID, json);
        }

        private void LoadInventory()
        {
            if (LuduInteraction.Runtime.Core.SaveManager.Instance == null) return;

            string json = LuduInteraction.Runtime.Core.SaveManager.Instance.GetString(m_SaveID);
            if (string.IsNullOrEmpty(json)) return;

            InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);
            if (data != null && data.ItemIDs != null)
            {
                m_Items.Clear();
                
                // Loading all ItemData assets from Resources/Items
                ItemData[] allItems = Resources.LoadAll<ItemData>("Items");
                Debug.Log($"[Inventory] Loaded {allItems.Length} items from Resources/Items.");
                
                foreach (string id in data.ItemIDs)
                {
                    foreach (var itemAsset in allItems)
                    {
                        if (itemAsset != null && itemAsset.ItemID == id)
                        {
                            m_Items.Add(itemAsset);
                            Debug.Log($"[Inventory] Restored Item: {itemAsset.ItemName}");
                            break;
                        }
                    }
                }
                
                OnInventoryChanged?.Invoke();
            }
        }

        [System.Serializable]
        private class InventorySaveData
        {
            public List<string> ItemIDs = new List<string>();
        }

        #endregion
    }
}
