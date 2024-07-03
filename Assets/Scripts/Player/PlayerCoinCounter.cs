
namespace Treasure.Player
{
    using UnityEngine;
    using Treasure.EventBus;
    using Treasure.Inventory;
    using DG.Tweening;
    using TMPro;

    public class PlayerCoinCounter : MonoBehaviour, IEventReceiver<OnGainReward>, IEventReceiver<LoseCoinsEvent>
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

        public void OnEvent(LoseCoinsEvent e)
        {
            _counterText.DOComplete();
            _counterText.DOColor(Color.red, 0.5f).SetLoops(2, LoopType.Yoyo);
            SetCounter(-e.coinsAmount);
        }

        private void OnEnable()
        {
            EventBus<OnGainReward>.Register(this);
            EventBus<LoseCoinsEvent>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<OnGainReward>.UnRegister(this);
            EventBus<LoseCoinsEvent>.UnRegister(this);
        }

        private void SetCounter(int amount)
        {
            DOTween.To(()=> coins, x => coins = x, coins + amount, 0.5f).OnUpdate(()=>
            {
                if(coins <= 0) coins = 0;
                _counterText.SetText(coins.ToString("000000"));
            });
        }
    }
}