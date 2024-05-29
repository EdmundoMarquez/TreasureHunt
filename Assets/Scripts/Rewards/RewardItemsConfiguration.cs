namespace Treasure.Rewards
{
    using UnityEngine;
    using Treasure.Common;

    [CreateAssetMenu(fileName = "RewardItems", menuName = "Rewards/RewardItemsConfiguration", order = 0)]
    public class RewardItemsConfiguration : ScriptableObject
    {
        public ItemData[] Level1RewardItems;
        public int Level1RewardMaxCoins = 500;
    }
}