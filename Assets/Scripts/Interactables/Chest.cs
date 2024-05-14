namespace Treasure.Interactables
{
    using Treasure.EventBus;
    using Treasure.Common;
    using Treasure.Puzzle;
    using UnityEngine;
    using DG.Tweening;

    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private ChestData _chestData;
        [SerializeField] private GameObject _lockedSprite;
        [SerializeField] private GameObject _unlockedSprite;
        [SerializeField] private SpriteRenderer _treasureSprite;
        private WordData _wordToSolve;
        private bool _isUnlocked;
        private int _totalReward = 500;
        public bool CanInteract => !_isUnlocked;
        private Sequence UnlockSequence;

        private void Start()
        {
            _wordToSolve = _chestData.RandomizeWord();
        }

        public void Interact()
        {
            EventBus<OnShowPuzzle>.Raise(new OnShowPuzzle
            {
                chest = gameObject,
                puzzleWord = _wordToSolve
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
            EventBus<OnGainReward>.Raise(new OnGainReward
            {
                coinAmount = _totalReward
            });

            _treasureSprite.gameObject.SetActive(true);

            UnlockSequence = DOTween.Sequence();

            UnlockSequence.Append(_treasureSprite.transform.DOLocalMoveY(0.5f, 0.5f))
            .AppendInterval(0.25f)
            .Append(_treasureSprite.DOFade(0f, 0.2f));
        }
    }
}
