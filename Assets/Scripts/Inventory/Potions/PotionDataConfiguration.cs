namespace Treasure.Inventory.Potions
{
    using UnityEngine;
    using Treasure.Common;

    [CreateAssetMenu(fileName = "PotionDataConfiguration", menuName = "Items/PotionDataConfiguration", order = 2)]
    public class PotionDataConfiguration : ScriptableObject
    {
        public PotionData[] healingPotions;
        public PotionData[] staminaPotions;
    }
}