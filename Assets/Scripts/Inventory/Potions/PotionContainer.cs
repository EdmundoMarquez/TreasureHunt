namespace Treasure.Inventory.Potions
{
    using UnityEngine;
    using UnityEngine.UI;

    public class PotionContainer : MonoBehaviour
    {
        [SerializeField] private Button _potionButton = null;
        [SerializeField] private Image _potionIcon = null;

        public void Init(bool isEnabled, Sprite icon)
        {
            _potionButton.interactable = isEnabled;

            Color iconColor = _potionIcon.color;
            iconColor.a = isEnabled ? 1 : 0;
            _potionIcon.color = iconColor;

            _potionIcon.sprite = icon;
        }

    }

}