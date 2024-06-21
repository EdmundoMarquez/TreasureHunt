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
        [SerializeField] private TrapBuilder _trapBuilder = null;
        [SerializeField] private GameObject _lockedSprite;
        [SerializeField] private GameObject _unlockedSprite;
        [SerializeField] private SpriteRenderer _treasureSprite;
        [SerializeField] private MinimapIconDisplay _minimapIcon = null;
        private WordData _wordToSolve;
        private bool _canInteract;
        public bool CanInteract => _canInteract;
        private Sequence UnlockSequence;
        private Collider2D _collider;

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
            _wordToSolve = ChestData.RandomizeWord();
            CurrentTries = ChestData.Tries;
            _minimapIcon.Init(_lockedSprite.GetComponent<SpriteRenderer>().sprite);
            _trapBuilder.Init(CurrentTries);
            _collider.enabled = true;
            
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
            _canInteract = !unlock;
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
            _trapBuilder.ActivateGeneratedTraps();
            _canInteract = false;
        }

        // public void PlayFloatingTreasureAnimation(Sprite rewardIcon)
        // {
        //     _treasureSprite.sprite = rewardIcon;
        //     _treasureSprite.gameObject.SetActive(true);

        //     UnlockSequence = DOTween.Sequence();

        //     UnlockSequence.Append(_treasureSprite.transform.DOLocalMoveY(0.5f, 0.5f))
        //     .AppendInterval(0.25f)
        //     .Append(_treasureSprite.DOFade(0f, 0.2f));
        // }
    }
}
