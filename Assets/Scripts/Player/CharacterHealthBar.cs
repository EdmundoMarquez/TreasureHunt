namespace Treasure.Player
{
    using UnityEngine;

    public class CharacterHealthBar : MonoBehaviour
    {
        [SerializeField] private CharacterHealthController _healthController = null;
        [SerializeField] private RectTransform _fillBar;
        private float maxFillSize;

        public void Init()
        {
            maxFillSize = _fillBar.sizeDelta.x;
            UpdateHealthBar();
        }

        private void OnEnable()
        {
            _healthController.onDead += UpdateHealthBar;
            _healthController.onDamageFeedback += UpdateHealthBar;
        }

        private void OnDisable()
        {
            _healthController.onDead -= UpdateHealthBar;
            _healthController.onDamageFeedback -= UpdateHealthBar;
        }

        private void UpdateHealthBar()
        {
            float updatedFillSize = _healthController.Health * maxFillSize / _healthController.MaxHealth;
            _fillBar.sizeDelta = new Vector2(updatedFillSize, _fillBar.sizeDelta.y);
        }
    }
}