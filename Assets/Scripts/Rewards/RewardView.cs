namespace Treasure.Rewards
{
    using Treasure.Common;
    using Treasure.EventBus;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using DG.Tweening;

    public class RewardView : MonoBehaviour, IEventReceiver<OnOpenChest>
    {
        [SerializeField] private CanvasGroup _canvasGroup = null;
        [SerializeField] private RewardController _rewardController = null;
        [SerializeField] private Image _itemIcon = null;
        [SerializeField] private TMP_Text _itemText = null;
        private RewardItem generatedItem;
        public void OnEvent(OnOpenChest e)
        {
            generatedItem = _rewardController.RandomizeReward(e.itemPlacementPosition, e.itemParent);
            if(generatedItem == null) return; //Don't show screen if reward is coins or nothing

            _itemText.SetText(generatedItem.Id);
            _itemIcon.sprite = generatedItem.Icon;
            ToggleVisibility(true);
        }

        private void ToggleVisibility(bool toggle)
        {
            _canvasGroup.alpha = toggle ? 1f : 0f;
            _canvasGroup.blocksRaycasts = toggle;
            _canvasGroup.interactable = toggle;
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