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
    public string _puzzleWord;

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
        GenerateLetterSlots();
    }

    private void GenerateLetterSlots()
    {
        for (int i = 0; i < _slotsParent.childCount; i++)
        {
            Destroy(_slotsParent.GetChild(i).gameObject);
        }

        float adjustedPanelSizeX = _slotsSize * _puzzleWord.Length + _slotsSpacing;
        float adjustedPanelSizeY = _lockPanel.sizeDelta.y;

        foreach(var character in _puzzleWord)
        {
            if(_puzzleWord.Length >= 9)
            {
                AdjustPanelSize(ref adjustedPanelSizeX, ref adjustedPanelSizeY);
            }

            if (character == '_')
            {
                DropSlot dropSlot = Object.Instantiate(_dropSlotTemplate, _slotsParent);
                dropSlot.Init("X");
                dropSlot.gameObject.name = "X Slot";
                continue;
            }

            LetterSlot letterSlot = Object.Instantiate(_letterSlotTemplate, _slotsParent);
            letterSlot.Init(character.ToString());
            letterSlot.gameObject.name = $"{character} Slot";
        }

        _lockPanel.sizeDelta = new Vector2(adjustedPanelSizeX, adjustedPanelSizeY);
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
