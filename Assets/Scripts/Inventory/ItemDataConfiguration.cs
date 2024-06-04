namespace Treasure.Inventory
{
    using UnityEngine;
    using Treasure.Common;
    using System.Collections.Generic;

    [CreateAssetMenu(fileName = "ItemDataConfiguration", menuName = "Items/ItemDataConfiguration", order = 2)]
    public class ItemDataConfiguration : ScriptableObject
    {
        public PotionData[] HealingPotions;
        public PotionData[] SpeedPotions;
        public PotionData[] InvisibilityPotions;
        public PotionData RevivePotion;
        public SwordData[] Swords;

        public PotionData[] GetAllPotions()
        {
            List<PotionData> allPotions = new List<PotionData>();

            foreach (var potion in HealingPotions)
                allPotions.Add(potion);

            foreach (var potion in SpeedPotions)
                allPotions.Add(potion);

            foreach (var potion in InvisibilityPotions)
                allPotions.Add(potion);

            // allPotions.Add(RevivePotion);
            return allPotions.ToArray();
        }
    }
}