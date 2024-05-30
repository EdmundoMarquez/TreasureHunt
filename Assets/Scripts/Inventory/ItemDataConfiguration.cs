namespace Treasure.Inventory
{
    using UnityEngine;
    using Treasure.Common;

    [CreateAssetMenu(fileName = "ItemDataConfiguration", menuName = "Items/ItemDataConfiguration", order = 2)]
    public class ItemDataConfiguration : ScriptableObject
    {
        public PotionData[] HealingPotions;
        public SwordData[] Swords;
    }
}