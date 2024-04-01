using DG.Tweening;
using Treasure.EventBus;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class ShowPuzzleView : MonoBehaviour, IEventReceiver<OnShowPuzzle>
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _exitButton;
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private RectTransform[] _draggableKeyItems;
    [SerializeField] private RectTransform _slotsParent;
    [SerializeField] private RectTransform _lockPanel;
    [SerializeField] private LetterSlot _letterSlotTemplate;
    [SerializeField] private DropSlot _dropSlotTemplate;
    [Header("Configurations")]
    [SerializeField] private float _slotsSize = 60;
    [SerializeField] private float _slotsSpacing = 160;
    [SerializeField] private Vector2 _minLockPanelSize = new Vector2(550, 50);
    private WordData _puzzleWord;

    private void Start()
    {
        _exitButton.onClick.AddListener(()=> ToggleVisibility(false));
    }

    private void ToggleVisibility(bool toggle)
    {
        if(toggle) RefreshDraggableKeys();

        _canvasGroup.DOFade(toggle ? 1f : 0f, 0.5f);
        _canvasGroup.interactable = toggle;
        _canvasGroup.blocksRaycasts = toggle;

    }

    private void RefreshDraggableKeys()
    {
        string[] ids = _inventoryController.GetAllKeys();

        for (int i = 0; i < ids.Length; i++)
        {
            _draggableKeyItems[i].gameObject.SetActive(_inventoryController.GetKeyById(ids[i]));
        }
    }

    public void OnEvent(OnShowPuzzle e)
    {
        ToggleVisibility(true);
        _puzzleWord = e.puzzleWord;
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
        foreach(var letter in _puzzleWord.Word)
        {
            letterPosition++;
            if(_puzzleWord.Word.Length >= 9)
            {
                AdjustPanelSize(ref adjustedPanelSizeX, ref adjustedPanelSizeY);
            }

            if (ShouldGenerateDropSlot(letter, letterPosition))
            {
                DropSlot dropSlot = Object.Instantiate(_dropSlotTemplate, _slotsParent);
                dropSlot.Init(letter.ToString());
                dropSlot.gameObject.name = $"{letter} Slot";
                continue;
            }

            LetterSlot letterSlot = Object.Instantiate(_letterSlotTemplate, _slotsParent.);
            letterSlot.Init(letter.ToString());
            letterSlot.gameObject.name = $"{letter} Slot";
        }

        _lockPanel.sizeDelta = new Vector2(adjustedPanelSizeX, adjustedPanelSizeY);
    }

    private bool ShouldGenerateDropSlot(char letter, int letterPosition)
    {
        foreach (var missingLetter in _puzzleWord.MissingLetters)
        {
            if(letter != missingLetter.letter) continue;
            if(letterPosition == missingLetter.position) return true;
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
