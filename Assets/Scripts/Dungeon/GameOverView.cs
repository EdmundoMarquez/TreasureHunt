namespace Treasure.Dungeon
{
    using UnityEngine;
    using Treasure.Inventory;
    using Treasure.Common;
    using Treasure.EventBus;
    using DG.Tweening;
    using System.Collections;
    using TMPro;

    public class GameOverView : MonoBehaviour, IEventReceiver<GameOverEvent>
    {
        [SerializeField] private CanvasGroup _canvasGroup = null;
        [SerializeField] private CanvasGroupFacade _canvasFacade = null;
        [SerializeField] private DungeonLevelData _dungeonLevelData = null;
        [SerializeField] private InventoryData _inventoryData = null;
        [SerializeField] private GameTimeTracker _timeTracker = null;
        [SerializeField] private TMP_Text _slayerText = null;
        [SerializeField] private TMP_Text _timePlayingText = null;
        [SerializeField] private TMP_Text _levelText = null;
        [SerializeField] private TMP_Text _coinsText = null;

        public void OnEvent(GameOverEvent e)
        {
            _slayerText.SetText($"<color=red>{e.instigatorId}</color>\n dio el golpe de gracia.");
            _timePlayingText.SetText($"{(int)(_timeTracker.GameTime / 60)}:{_timeTracker.GameTime % 60}");
            _levelText.SetText($"Nivel {_dungeonLevelData.currentLevel}");
            _coinsText.SetText(_inventoryData.Coins.ToString("00000"));
            StartCoroutine(ShowGameOver_Timer());
        }

        private IEnumerator ShowGameOver_Timer()
        {
            Time.timeScale = 0.5f;
            yield return new WaitForSeconds(3f);
            ToggleVisibility(true);
        }

        public void ToggleVisibility(bool toggle)
        {
            Time.timeScale = toggle ? 0f : 1f;
            _canvasGroup.DOFade(toggle ? 1f: 0f, 0.3f).SetUpdate(true);
            _canvasGroup.interactable = toggle;
            _canvasGroup.blocksRaycasts = toggle;

            _canvasFacade.ToggleVisibility(!toggle);
        }

        private void OnEnable()
        {
            EventBus<GameOverEvent>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<GameOverEvent>.UnRegister(this);
        }
    }
}