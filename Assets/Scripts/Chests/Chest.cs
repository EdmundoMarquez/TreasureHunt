namespace Treasure.Chests
{
    using Treasure.EventBus;
    using Treasure.Common;
    using UnityEngine;
    using DG.Tweening;

    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private TrapBuilder _trapBuilder = null;
        [SerializeField] private GameObject _lockedChestSprite;
        [SerializeField] private GameObject _unlockedChestSprite;
        // [SerializeField] private SpriteRenderer _treasureSprite;
        [SerializeField] private SpriteColorModifier _lockColorModifier = null;
        [SerializeField] private MinimapIconDisplay _minimapIcon = null;
        private ChestData _chestData;
        private WordData _wordToSolve;
        private bool _canInteract = true;
        public bool CanInteract => _canInteract;
        private Sequence UnlockSequence;
        private Collider2D _collider;
        public int Tries {get; set;}
        public int CurrentTries {get; set;}

        public void Init(ChestData chestData)
        {
            _chestData = chestData;
            _collider = GetComponent<Collider2D>();
            _wordToSolve = _chestData.WordData;
            Tries = _chestData.Tries;
            CurrentTries = _chestData.Tries;
            _trapBuilder.Init((int)_chestData.trapType);
            _lockColorModifier.ChangeColor(_chestData.Tries);
            _minimapIcon.Init(_lockedChestSprite.GetComponent<SpriteRenderer>().sprite);
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
            _lockedChestSprite.SetActive(!unlock);
            _unlockedChestSprite.SetActive(unlock);

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
