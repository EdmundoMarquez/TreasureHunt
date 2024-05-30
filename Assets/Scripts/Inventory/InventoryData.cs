namespace Treasure.Inventory
{
    using UnityEngine;
    using Treasure.Common;
    using System.Collections.Generic;
    using Treasure.Inventory.Potions;

    [CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/InventoryData", order = 1)]
    public class InventoryData : ScriptableObject
    {
        public int Coins;
        public KeyData[] Keys;
        public string EquippedSword;
        public List<SwordInventoryData> StoredSwords = new List<SwordInventoryData>();
        public List<PotionInventoryData> StoredPotions = new List<PotionInventoryData>();
    }
}