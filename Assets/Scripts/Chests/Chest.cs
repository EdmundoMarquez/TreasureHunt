namespace Treasure.Chests
{
    using Treasure.EventBus;
    using Treasure.Common;
    using UnityEngine;
    using DG.Tweening;

    public class Chest : MonoBehaviour, IInteractable
    {
        public ChestData ChestData;
        public int CurrentTries;
        [SerializeField] private GameObject _lockedSprite;
        [SerializeField] private GameObject _unlockedSprite;
        [SerializeField] private SpriteRenderer _treasureSprite;
        [SerializeField] private MinimapIconDisplay _minimapIcon = null;
        private WordData _wordToSolve;
        private bool _isUnlocked;
        public bool CanInteract => !_isUnlocked;
        private Sequence UnlockSequence;

        private void Start()
        {
            _wordToSolve = ChestData.RandomizeWord();
            CurrentTries = ChestData.Tries;
            _minimapIcon.Init(_lockedSprite.GetComponent<SpriteRenderer>().sprite);

            EventBus<OnChestGenerated>.Raise(new OnChestGenerated());
        }

        public void Interact()
        {
            EventBus<OnShowPuzzle>.Raise(new OnShowPuzzle
            {
                chest = gameObject,
                puzzleWord = _wordToSolve,
                currentTries = CurrentTries
            });
        }


        public void ToggleLock(bool unlock)
        {
            _isUnlocked = unlock;
            _lockedSprite.SetActive(!unlock);
            _unlockedSprite.SetActive(unlock);

            if (unlock) OnUnlockChest();
        }

        private void OnUnlockChest()
        {
            EventBus<OnOpenChest>.Raise(new OnOpenChest
            {
                itemParent = transform,
                itemPlacementPosition = transform.position
            });
        }

        public void ActivateTrap()
        {
            CurrentTries = 0;
        }

        public void PlayFloatingTreasureAnimation(Sprite rewardIcon)
        {
            _treasureSprite.sprite = rewardIcon;
            _treasureSprite.gameObject.SetActive(true);

            UnlockSequence = DOTween.Sequence();

            UnlockSequence.Append(_treasureSprite.transform.DOLocalMoveY(0.5f, 0.5f))
            .AppendInterval(0.25f)
            .Append(_treasureSprite.DOFade(0f, 0.2f));
        }
    }
}
