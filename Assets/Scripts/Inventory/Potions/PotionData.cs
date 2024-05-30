namespace Treasure.Inventory
{
    using UnityEngine;
    using Treasure.Common;
    using System;

    [CreateAssetMenu(fileName = "PotionData", menuName = "Items/PotionData", order = 1)]
    public class PotionData : ScriptableObject
    {
        public Sprite PotionImage;
        public DataProperty Properties;
    }

    [Serializable]
    public class PotionInventoryData
    {
        public Sprite PotionImage;
        public DataProperty Properties;
        public int Quantity;
    }
}