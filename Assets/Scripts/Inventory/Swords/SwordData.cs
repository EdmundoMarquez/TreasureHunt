namespace Treasure.Inventory
{
    using UnityEngine;
    using Treasure.Common;

    [CreateAssetMenu(fileName = "SwordData", menuName = "Items/SwordData", order = 0)]
    public class SwordData : ScriptableObject
    {
        public ObjectId SwordId;
        public Sprite SwordImage;
        public DataProperty[] Damage;
    }

    [System.Serializable]
    public class SwordInventoryData
    {
        public string SwordId;
        public Sprite SwordImage;
        public DataProperty[] Damage;
        public int Quantity;
    }
}