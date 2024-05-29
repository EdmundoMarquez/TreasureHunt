namespace Treasure.Puzzle
{

    using DG.Tweening;
    using Treasure.EventBus;
    using Treasure.Inventory;
    using Treasure.Common;
    using Treasure.Interactables;
    using UnityEngine;
    using UnityEngine.UI;

    public class LockPuzzleView : MonoBehaviour, IEventReceiver<OnShowPuzzle>
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _exitButton;
        [SerializeField] private InventoryController _inventoryController;
        [SerializeField] private RectTransform[] _draggableKeyItems;
        [SerializeField] private RectTransform _slotsParent;
        [SerializeField] private RectTransform _lockPanel;
        [SerializeField] private LetterSlot _letterSlotTemplate;
        [SerializeField] private DropSlot _dropSlotTemplate;
        [SerializeField] private CanvasGroupFacade _canvasFacade = null;
        [Header("Configurations")]
        [SerializeField] private float _slotsSize = 60;
        [SerializeField] private float _slotsSpacing = 160;
        [SerializeField] private Vector2 _minLockPanelSize = new Vector2(550, 50);
        private WordData _puzzleWord;
        private Chest _currentChest;
        private int _totalNumberOfLocks;
        private int _numberOfLocksUnlocked;

        private void Start()
        {
            _exitButton.onClick.AddListener(() => ToggleVisibility(false));
        }

        private void ToggleVisibility(bool toggle)
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
            string[] ids = _inventoryController.GetAllKeyIds();

            for (int i = 0; i < ids.Length; i++)
            {
                _draggableKeyItems[i].gameObject.SetActive(_inventoryController.GetKeyById(ids[i]));
            }
        }

        public void OnEvent(OnShowPuzzle e)
        {
            ToggleVisibility(true);

            _puzzleWord = e.puzzleWord;
            _currentChest = e.chest.GetComponent<Chest>();

            _numberOfLocksUnlocked = 0;
            _totalNumberOfLocks = 0;

            GenerateLetterSlots();
        }

        private void GenerateLetterSlots()
        {
            for (int i = 0; i < _slotsParent.childCount; i++)
            {
                Destroy(_slotsParent.GetChild(i).gameObject);
            }


            float adjustedPanelSizeX = _slotsSize * _puzzleWord.Word.Length + _slotsSpacing;
            float adjustedPanelSizeY = _lockPanel.sizeDelta.y;

            int letterPosition = 0;
            foreach (var letter in _puzzleWord.Word)
            {
                letterPosition++;
                if (_puzzleWord.Word.Length >= 9)
                {
                    AdjustPanelSize(ref adjustedPanelSizeX, ref adjustedPanelSizeY);
                }

                if (ShouldGenerateDropSlot(letter, letterPosition))
                {
                    DropSlot dropSlot = Object.Instantiate(_dropSlotTemplate, _slotsParent);
                    dropSlot.Init(letter.ToString());
                    dropSlot.gameObject.name = $"{letter} Slot";

                    _totalNumberOfLocks++;
                    dropSlot.onUnlocked += OnUnlockingSlot;

                    continue;
                }

                LetterSlot letterSlot = Object.Instantiate(_letterSlotTemplate, _slotsParent);
                letterSlot.Init(letter.ToString());
                letterSlot.gameObject.name = $"{letter} Slot";
            }

            _lockPanel.sizeDelta = new Vector2(adjustedPanelSizeX, adjustedPanelSizeY);
        }

        private void OnUnlockingSlot()
        {
            _numberOfLocksUnlocked++;

            if (_numberOfLocksUnlocked < _totalNumberOfLocks) return;
            _currentChest.ToggleLock(true);
            ToggleVisibility(false);
        }

        private bool ShouldGenerateDropSlot(char letter, int letterPosition)
        {
            foreach (var missingLetter in _puzzleWord.MissingLetters)
            {
                if (letter != missingLetter.letter) continue;
                if (letterPosition == missingLetter.position) return true;
            }
            return false;
        }

        private void AdjustPanelSize(ref float adjustedPanelSizeX, ref float adjustedPanelSizeY)
        {
            float reductionMargin = 2;
            adjustedPanelSizeX -= reductionMargin;
            adjustedPanelSizeY -= reductionMargin / 2;

            adjustedPanelSizeX = Mathf.Clamp(adjustedPanelSizeX, _minLockPanelSize.x, 700);
            adjustedPanelSizeY = Mathf.Clamp(adjustedPanelSizeY, _minLockPanelSize.y, 100);
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