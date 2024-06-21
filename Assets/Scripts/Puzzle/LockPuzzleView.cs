namespace Treasure.Puzzle
{

    using DG.Tweening;
    using Treasure.EventBus;
    using Treasure.Inventory;
    using Treasure.Common;
    using Treasure.Chests;
    using UnityEngine;
    using UnityEngine.UI;

    public class LockPuzzleView : MonoBehaviour, IEventReceiver<OnShowPuzzle>
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _exitButton;
        [SerializeField] private InventoryController _inventoryController;
        [SerializeField] private RectTransform[] _draggableKeyItems;
        [SerializeField] private CanvasGroupFacade _canvasFacade = null;
        [SerializeField] private PuzzleSlotsGenerator _puzzleSlotsGenerator = null;
        [SerializeField] private PuzzleTryCounter _puzzleTryController = null;
        private WordData _puzzleWord;
        private Chest _currentChest;
        private int _totalNumberOfLocks;
        private int _numberOfLocksUnlocked;

        private void Start()
        {
            _exitButton.onClick.AddListener(() => ToggleVisibility(false));
        }

        public void ToggleVisibility(bool toggle)
        {
            if (toggle) RefreshDraggableKeys();

            _canvasGroup.DOFade(toggle ? 1f : 0f, 0.5f).SetUpdate(true);
            _canvasGroup.interactable = toggle;
            _canvasGroup.blocksRaycasts = toggle;

            _canvasFacade.ToggleVisibility(!toggle);
            Time.timeScale = toggle ? 0f : 1f;
        }

        private void RefreshDraggableKeys()
        {
            var keys = _inventoryController.GetAllKeys();

            for (int i = 0; i < keys.Length; i++)
            {
                _draggableKeyItems[i].gameObject.SetActive(keys[i].Unlocked);
            }
        }

        public void OnEvent(OnShowPuzzle e)
        {
            ToggleVisibility(true);

            _puzzleWord = e.puzzleWord;
            _currentChest = e.chest.GetComponent<Chest>();

            _numberOfLocksUnlocked = 0;
            _totalNumberOfLocks = 0;

            DropSlot[] dropSlots = _puzzleSlotsGenerator.GenerateLetterSlots(_puzzleWord);
            
            foreach (var slot in dropSlots)
            {
                _totalNumberOfLocks++;
                slot.onUnlocked += OnUnlockingSlot;
            }
            
            _puzzleTryController.Init(this, _currentChest);
        }

        private void OnUnlockingSlot()
        {
            _numberOfLocksUnlocked++;

            if (_numberOfLocksUnlocked < _totalNumberOfLocks) return;
            _currentChest.ToggleLock(true);

            //Hide lock puzzle view
            _canvasGroup.DOFade(0f, 0.5f).SetUpdate(true);
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private void OnEnable()
        {
            EventBus<OnShowPuzzle>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<OnShowPuzzle>.UnRegister(this);
        }
    }

}