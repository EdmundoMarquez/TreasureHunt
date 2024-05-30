namespace Treasure.Inventory
{
    using UnityEngine;
    using Treasure.Common;

    [CreateAssetMenu(fileName = "KeyData", menuName = "KeyData", order = 1)]
    public class KeyData : ScriptableObject
    {
        [SerializeField] ObjectId _objectId;
        public string Id => _objectId.Value;
        public bool IsUnlocked;
    }

    [System.Serializable]
    public class KeyInventoryData
    {
        public string Id;
        public bool Unlocked;
    }
}