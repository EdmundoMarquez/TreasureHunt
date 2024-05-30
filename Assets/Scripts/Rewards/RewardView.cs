namespace Treasure.Rewards
{
    using Treasure.Common;
    using Treasure.Inventory;
    using Treasure.EventBus;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using DG.Tweening;

    public class RewardView : MonoBehaviour, IEventReceiver<OnOpenChest>
    {
        [SerializeField] private CanvasGroupFacade _canvasFacade = null;
        [SerializeField] private CanvasGroup _canvasGroup = null;
        [SerializeField] private RewardController _rewardController = null;
        [SerializeField] private InventoryController _inventoryController = null;
        [SerializeField] private Image _itemIcon = null;
        [SerializeField] private TMP_Text _itemText = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private Button _takeButton = null;
        private RewardItem generatedItem;

        private void Start()
        {
            _takeButton.onClick.AddListener(() => TakeItem(generatedItem.Data));
            _closeButton.onClick.AddListener(() => 
            {
                generatedItem.EnablePickable();
                ToggleVisibility(false);
            });
        }

        public void TakeItem(ItemData itemData)
        {
            switch (itemData.pickableType)
            {
                case PickableTypes.Potion:
                    DataProperty newDataProperty = new DataProperty();
                    newDataProperty.propertyId = itemData.pickableId;
                    newDataProperty.amount = 1;

                    EventBus<AddPotionItem>.Raise(new AddPotionItem
                    {
                        potionProperties = newDataProperty,
                        potionObject = generatedItem.gameObject
                    });
                    break;

                case PickableTypes.Sword:
                    EventBus<ConfirmAddSwordItem>.Raise(new ConfirmAddSwordItem
                    {
                        itemId = itemData.pickableId.Value,
                        swordObject = generatedItem.gameObject
                    });
                    break;
                case PickableTypes.Key:
                    EventBus<AddKeyItem>.Raise(new AddKeyItem
                    {
                        itemId = itemData.pickableId.Value
                    });
                    break;
                case PickableTypes.None:
                default:
                    break;
            }

            ToggleVisibility(false);
        }

        public void OnEvent(OnOpenChest e)
        {
            generatedItem = _rewardController.RandomizeReward(e.itemPlacementPosition, e.itemParent);
 
            //Don't show screen if reward is coins or nothing
            if(generatedItem == null)
            {
                _canvasFacade.ToggleVisibility(true);
                Time.timeScale = 1f;
                return; 
            } 

            _itemText.SetText(generatedItem.Id);
            _itemIcon.sprite = generatedItem.Icon;
            ToggleVisibility(true);

            if(generatedItem.Data.pickableType == PickableTypes.Potion)
            {
                if(_inventoryController.IsPotionSpaceFull())
                {
                    _takeButton.gameObject.SetActive(false);
                }
            }

        }

        private void ToggleVisibility(bool toggle)
        {
            _canvasGroup.alpha = toggle ? 1f : 0f;
            _canvasGroup.blocksRaycasts = toggle;
            _canvasGroup.interactable = toggle;

            _canvasFacade.ToggleVisibility(!toggle);
            Time.timeScale = toggle ? 0f : 1f;
        }

        private void OnEnable()
        {
            EventBus<OnOpenChest>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<OnOpenChest>.UnRegister(this);
        }
    }
}