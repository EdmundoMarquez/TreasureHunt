namespace Treasure.Rewards
{
    using UnityEngine;
    using Treasure.EventBus;
    using Random = UnityEngine.Random;

    public class RewardController : MonoBehaviour
    {
        [SerializeField] private RewardItemsConfiguration _rewardsConfiguration = null;
        [SerializeField] private RewardPlacer _rewardPlacer = null;
        private float randomizedValue;

        public RewardItem RandomizeReward(Vector3 placementPosition, Transform itemParent)
        {
            RewardItem spawnedItem = null;
            randomizedValue = Random.value;

            if(randomizedValue > 0.5f)
            {
                //Gain an item
                var randomizedItem = _rewardsConfiguration.Level1RewardItems[Random.Range(0, _rewardsConfiguration.Level1RewardItems.Length)];
                spawnedItem = _rewardPlacer.PlaceItem(randomizedItem, itemParent).GetComponent<RewardItem>();
            }
            else if(randomizedValue > 0.3f)
            {
                //Gain coins
                EventBus<OnGainReward>.Raise(new OnGainReward
                {
                    coinAmount = Random.Range(10, _rewardsConfiguration.Level1RewardMaxCoins)
                });
            }
            return spawnedItem;
        }
    }

}