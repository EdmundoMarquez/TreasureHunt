
namespace Treasure.Player
{
    using UnityEngine;
    using Treasure.EventBus;
    using Treasure.Inventory;
    using DG.Tweening;
    using TMPro;

    public class PlayerCoinCounter : MonoBehaviour, IEventReceiver<OnGainReward>
    {
        [SerializeField] private InventoryData _inventoryData = null;
        [SerializeField] private TMP_Text _counterText = null;
        private int coins = 0;

        private void Start()
        {
            coins = _inventoryData.Coins;
            SetCounter(coins);
        }

        public void OnEvent(OnGainReward e)
        {
            SetCounter(e.coinAmount);
        }

        private void OnEnable()
        {
            EventBus<OnGainReward>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<OnGainReward>.UnRegister(this);
        }

        private void SetCounter(int amount)
        {
            DOTween.To(()=> coins, x => coins = x, coins + amount, 0.5f).OnUpdate(()=>
            {
                _counterText.SetText(coins.ToString("000000"));
            });
        }
    }
}