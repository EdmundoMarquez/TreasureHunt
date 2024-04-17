namespace Treasure.Damageables
{

    using UnityEngine;
    using DG.Tweening;
    using Treasure.Common;
    using Pathfinding;

    public class Bat : MonoBehaviour
    {
        [SerializeField] private DamageableHealthController _healthController = null;
        [SerializeField] private DamageInstigator _damageInstigator = null;
        [SerializeField] private Collider2D _collider = null;
        [SerializeField] private SpriteRenderer _batSprite = null;
        [SerializeField] private Transform _target = null;
        [SerializeField] private AIPath _pathAgent = null;
        [SerializeField] private DataProperty[] _damageProperties;

        private void Start()
        {
            _damageInstigator.Init(_damageProperties);
        }

        private void OnEnable()
        {
            _healthController.onDestroyDamageable += OnDead;
            _healthController.onDamageFeedback += OnDamage;
        }

        private void OnDisable()
        {
            _healthController.onDestroyDamageable -= OnDead;
            _healthController.onDamageFeedback -= OnDamage;
        }

        private void OnDamage()
        {
            _batSprite.transform.DOShakePosition(0.4f, 0.5f);
        }

        private void OnDead()
        {
            _batSprite.transform.DOShakePosition(0.4f, 0.5f);
            _batSprite.DOFade(0f, 0.4f);
            _collider.enabled = false;
            _pathAgent.enabled = false;
        }

        private void Update()
        {
            _pathAgent.destination = _target.position;
        }
    }
}
