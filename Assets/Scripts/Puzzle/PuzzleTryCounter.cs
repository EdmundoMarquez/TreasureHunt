namespace Treasure.Puzzle
{
    using UnityEngine;
    using Treasure.Chests;
    using Treasure.EventBus;
    using DG.Tweening;
    using UnityEngine.UI;

    public class PuzzleTryCounter : MonoBehaviour, IEventReceiver<OnFailPuzzleTry>
    {
        [SerializeField] private Image[] _tryCounterIcons;
        private Chest _currentChest;
        private LockPuzzleView _puzzleView;

        public void Init(LockPuzzleView puzzleView, Chest chest)
        {
            _puzzleView = puzzleView;
            _currentChest = chest;

            foreach (var icon in _tryCounterIcons)
                icon.gameObject.SetActive(false);

            ShowRemainingTries();
        }

        public void OnEvent(OnFailPuzzleTry e)
        {
            if(_currentChest.CurrentTries == 0) return;
            if(--_currentChest.CurrentTries < 1)
            {
                _currentChest.ActivateTrap();
                _puzzleView.ToggleVisibility(false);

            }

            ShowRemainingTries();
        }

        private void ShowRemainingTries()
        {
            for (int i = 0; i < _currentChest.ChestData.Tries; i++)
            {
                _tryCounterIcons[i].gameObject.SetActive(true);
                _tryCounterIcons[i].DOFade(i < _currentChest.CurrentTries ? 1f : 0.5f, 0.3f).SetUpdate(true); 
            }
        }

        private void OnEnable()
        {
            EventBus<OnFailPuzzleTry>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<OnFailPuzzleTry>.UnRegister(this);
        }
    }
}