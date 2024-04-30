namespace Treasure.Inventory.Potions
{
    using UnityEngine;
    using UnityEngine.UI;
    using Treasure.EventBus;

    public class PotionContainer : MonoBehaviour
    {
        [SerializeField] private Button _potionButton = null;
        [SerializeField] private Image _potionIcon = null;
        private string _potionId;

        private void Start() 
        {
            _potionButton.onClick.AddListener(OnContainerPressed);
        }

        public void Init(bool isEnabled, Sprite icon, string id = "")
        {
            _potionButton.interactable = isEnabled;

            Color iconColor = _potionIcon.color;
            iconColor.a = isEnabled ? 1 : 0;
            _potionIcon.color = iconColor;

            _potionIcon.sprite = icon;
            _potionId = id;
        }

        private void OnContainerPressed()
        {
            EventBus<ThrowPotionItem>.Raise(new ThrowPotionItem
            {
                potionId = _potionId
            });
        }
    }

}