namespace Treasure.Damageables
{

    using UnityEngine;
    using DG.Tweening;
    using Treasure.Common;

    public class Arrow : MonoBehaviour
    {
        [SerializeField] private DamageableHealthController _healthController = null;
        [SerializeField] private DataProperty[] _damageProperties;
        [SerializeField] private DamageInstigator _damageInstigator = null;
        [SerializeField] private Collider2D _collider = null;
        [SerializeField] private SpriteRenderer _arrowSprite = null;
        [SerializeField] private float _moveSpeed = 2;
        private Vector2 _targetPosition;
        private bool _canTick;
        private float _autoDestroyWaitTime = 10f;

        private void Awake()
        {
            _damageInstigator.Init(_damageProperties);
        }

        public void Init(Vector2 moveDirection)
        {
            _targetPosition = moveDirection;
            _canTick = true;
            Destroy(gameObject, _autoDestroyWaitTime);
        }

        private void OnEnable()
        {
            _healthController.onDestroyDamageable += OnDead;
            _damageInstigator.onHit += OnHit;
        }

        private void OnDisable()
        {
            _healthController.onDestroyDamageable -= OnDead;
            _damageInstigator.onHit -= OnHit;
        }

        private void OnDead()
        {
            _arrowSprite.gameObject.SetActive(false);
            _canTick = false;
        }

        private void Update()
        {
            if(!_canTick) return;

            float step = Time.deltaTime * _moveSpeed;
            transform.position += transform.up * step;
        }

        public void OnHit()
        {
            _arrowSprite.gameObject.SetActive(false);
            _canTick = false;
        }
    }
}
