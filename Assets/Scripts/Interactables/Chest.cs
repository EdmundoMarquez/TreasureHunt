namespace Treasure.Interactables
{
    using Treasure.EventBus;
    using Treasure.Common;
    using Treasure.Puzzle;
    using Treasure.Player;
    using UnityEngine;

    public class Chest : MonoBehaviour, IInteractable
    {
        // [SerializeField] private ObjectId _requiredCharacter;
        [SerializeField] private ChestData _chestData;
        [SerializeField] private GameObject _lockedSprite;
        [SerializeField] private GameObject _unlockedSprite;
        private WordData _wordToSolve;
        private bool _isUnlocked;
        public bool CanInteract => !_isUnlocked;

        private void Start()
        {
            _wordToSolve = _chestData.RandomizeWord();
        }

        public void Interact()
        {
            EventBus<OnShowPuzzle>.Raise(new OnShowPuzzle{
                chest = gameObject,
                puzzleWord = _wordToSolve
            });
        }


        public void ToggleLock(bool unlock)
        {
            _isUnlocked = unlock;
            _lockedSprite.SetActive(!unlock);
            _unlockedSprite.SetActive(unlock);
        }
    }
}
