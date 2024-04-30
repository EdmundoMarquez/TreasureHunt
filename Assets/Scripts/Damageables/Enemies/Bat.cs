namespace Treasure.Damageables
{

    using UnityEngine;
    using DG.Tweening;
    using Treasure.Common;
    using Pathfinding;

    public class Bat : MonoBehaviour
    {
        [SerializeField] private DamageableHealthController _healthController = null;
        [SerializeField] private DataProperty[] _damageProperties;
        [SerializeField] private DamageInstigator _damageInstigator = null;
        [SerializeField] private Collider2D _collider = null;
        [SerializeField] private SpriteRenderer _batSprite = null;
        [SerializeField] private Transform _target = null;
        [SerializeField] private float _radius = 5;
        [SerializeField] private float _rotationSpeed = 2;
        private float _angle;

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
        }

        private void Update()
        {
            _angle += _rotationSpeed * Time.deltaTime;

            var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * _radius;
            transform.position = (Vector2)_target.position + offset;
        }
    }
}
