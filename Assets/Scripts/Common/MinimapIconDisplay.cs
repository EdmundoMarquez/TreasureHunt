namespace Treasure.Common
{
    using UnityEngine;
    using DG.Tweening;

    public class MinimapIconDisplay : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer minimapIcon;
        [SerializeField] private Color enabledColor = Color.white;
        [SerializeField] private Color disabledColor = Color.black;
        [SerializeField] private Collider2D _minimapCollider;

        public void Init(Sprite icon)
        {
            minimapIcon.sprite = icon;
        }

        private void ShowMinimapIcon()
        {
            _minimapCollider.enabled = false;
            minimapIcon?.DOColor(enabledColor, 1f);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                ShowMinimapIcon();
            }
        }
    }
}