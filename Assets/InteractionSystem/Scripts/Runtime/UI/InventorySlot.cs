using LuduInteraction.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace LuduInteraction.Runtime.UI
{
    /// <summary>
    /// Represents a single slot in the visual inventory.
    /// </summary>
    public class InventorySlot : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Image m_IconImage;
        [SerializeField] private GameObject m_EmptyBackground;
        
        private ItemData m_CurrentItem;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // Automatically find the Image component on this GameObject if not assigned in Inspector
            if (m_IconImage == null)
            {
                m_IconImage = GetComponent<Image>();
            }
        }

        #endregion

        #region Properties

        public ItemData CurrentItem => m_CurrentItem;
        public bool IsEmpty => m_CurrentItem == null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Assigns item data to this slot and updates visuals.
        /// </summary>
        public void AssignItem(ItemData item)
        {
            m_CurrentItem = item;
            
            if (m_CurrentItem != null && m_CurrentItem.Icon != null)
            {
                if (m_IconImage != null)
                {
                    m_IconImage.sprite = m_CurrentItem.Icon;
                    m_IconImage.enabled = true;
                }
                
                if (m_EmptyBackground != null) m_EmptyBackground.SetActive(false);
            }
            else
            {
                ClearSlot();
            }
        }

        /// <summary>
        /// Resets the slot.
        /// </summary>
        public void ClearSlot()
        {
            m_CurrentItem = null;
            if (m_IconImage != null)
            {
                m_IconImage.sprite = null;
               // m_IconImage.enabled = false;
            }
            if (m_EmptyBackground != null) m_EmptyBackground.SetActive(true);
        }

        #endregion
    }
}