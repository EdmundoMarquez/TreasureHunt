namespace Treasure.Damageables
{
    using UnityEngine;
    using DG.Tweening;

    public class Crate : MonoBehaviour
    {
        [SerializeField] private DamageableHealthController _healthController = null;
        [SerializeField] private Collider2D _collider = null;
        [SerializeField] private SpriteRenderer _crateSprite;

        private void OnEnable()
        {
            _healthController.onDestroyDamageable += DestroyCrate;
            _healthController.onDamageFeedback += OnDamage;
        }

        private void OnDisable()
        {
            _healthController.onDestroyDamageable -= DestroyCrate;
            _healthController.onDamageFeedback -= OnDamage;
        }

        private void OnDamage()
        {
            _crateSprite.transform.DOShakePosition(0.4f, 0.5f);
        }

        private void DestroyCrate()
        {
            _crateSprite.transform.DOShakePosition(0.4f, 0.5f);
            _crateSprite.DOFade(0f, 0.4f);
            _collider.enabled = false;
        }
    }
}
