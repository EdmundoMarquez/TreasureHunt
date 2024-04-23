namespace Treasure.Inventory.Potions
{
    using UnityEngine;
    using Treasure.Common;

    [CreateAssetMenu(fileName = "PotionData", menuName = "Items/PotionData", order = 1)]
    public class PotionData : ScriptableObject
    {
        public Sprite PotionImage;
        public DataProperty Properties;
    }
}