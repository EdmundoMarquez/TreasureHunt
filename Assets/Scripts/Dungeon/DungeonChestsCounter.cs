namespace Treasure.Dungeon
{
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    using TMPro;
    using UnityEngine.Events;
    using Treasure.EventBus;

    public class DungeonChestsCounter : MonoBehaviour, IEventReceiver<OnOpenChest>, IEventReceiver<OnCompletedDungeon>, IEventReceiver<OnChestGenerated>
    {   
        [SerializeField] private TMP_Text _counterText = null;
        private int _totalChestsOpenedInLevel = 0;
        private int _numberOfChestsInLevel;
        private int _totalChestsOpenedInRun = 0;
        public int ChestsOpenedInRun => _totalChestsOpenedInRun;

        private void UpdateCounter()
        {
            _counterText.SetText($"{_totalChestsOpenedInLevel}/{_numberOfChestsInLevel}");
        }

        public void OnEvent(OnCompletedDungeon e)
        {
            _totalChestsOpenedInLevel = 0;
            _numberOfChestsInLevel = 0;
        }

        public void OnEvent(OnOpenChest e)
        {
            _totalChestsOpenedInLevel++;
            _totalChestsOpenedInRun++;
            UpdateCounter();
        }

        public void OnEvent(OnChestGenerated e)
        {
            _numberOfChestsInLevel++;
            UpdateCounter();
        }

        private void OnEnable()
        {
            EventBus<OnCompletedDungeon>.Register(this);
            EventBus<OnChestGenerated>.Register(this);
            EventBus<OnOpenChest>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<OnCompletedDungeon>.UnRegister(this);
            EventBus<OnChestGenerated>.UnRegister(this);
            EventBus<OnOpenChest>.UnRegister(this);
        }
    }
}

