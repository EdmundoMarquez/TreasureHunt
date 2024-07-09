namespace Treasure.Damageables
{

    using UnityEngine;
    using DG.Tweening;
    using Treasure.Common;
    using Pathfinding;

    public class Bat : MonoBehaviour, IEnemy
    {
        [SerializeField] private DamageableHealthController _healthController = null;
        [SerializeField] private DataProperty[] _damageProperties;
        [SerializeField] private DamageInstigator _damageInstigator = null;
        [SerializeField] private Collider2D _collider = null;
        [SerializeField] private SpriteRenderer _batSprite = null;
        [SerializeField] private Transform _target = null;
        [SerializeField] private Transform _batTransform = null;
        [SerializeField] private float _radius = 5;
        [SerializeField] private float _rotationSpeed = 2;
        private float _angle;
        public Transform Self => _batTransform;
        public Transform Player { get; set; }
        public bool IsDead {get; set;}

        private void Awake() { _damageInstigator.Init(_damageProperties); }

        public void Init() {}

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

        public void OnDamage()
        {
            // _batSprite.transform.DOShakePosition(0.4f, 0.5f);
            _batSprite.DOColor(Color.red, 0.3f).OnComplete(()=>
            { _batSprite.DOColor(Color.white, 0.3f); });
        }

        public void OnDead()
        {
            _batSprite.transform.DOShakePosition(0.4f, 0.5f);
            _batSprite.DOFade(0f, 0.4f);
            _collider.enabled = false;

            IsDead = true;
        }

        public void OnRevive()
        {
            
        }

        private void Update()
        {
            if(IsDead) return;
            _angle += _rotationSpeed * Time.deltaTime;

            var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * _radius;
            _batTransform.position = (Vector2)_target.position + offset;
        }
    }
}
