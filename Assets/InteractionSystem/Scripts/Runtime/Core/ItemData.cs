using UnityEngine;

namespace LuduInteraction.Runtime.Core
{
    /// <summary>
    /// Base class for all inventory items, defined as ScriptableObjects.
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "LuduInteraction/Item")]
    public class ItemData : ScriptableObject
    {
        #region Fields

        [Tooltip("The unique identifier for this item.")]
        [SerializeField] private string m_ItemID;

        [Tooltip("The display name of the item.")]
        [SerializeField] private string m_ItemName;

        [Tooltip("Icon for UI display.")]
        [SerializeField] private Sprite m_Icon;

        #endregion

        #region Properties

        public string ItemID => m_ItemID;
        public string ItemName => m_ItemName;
        public Sprite Icon => m_Icon;

        #endregion
    }
}
