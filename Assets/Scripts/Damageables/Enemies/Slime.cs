namespace Treasure.Damageables
{

    using UnityEngine;
    using DG.Tweening;
    using Treasure.Common;
    using Pathfinding;
    using System.Collections.Generic;

    public class Slime : MonoBehaviour, IEnemy
    {
        [SerializeField] private DamageableHealthController _healthController = null;
        [SerializeField] private StateController _stateController = null;
        [SerializeField] private DataProperty[] _damageProperties;
        [SerializeField] private DamageInstigator _damageInstigator = null;
        [SerializeField] private Collider2D[] _colliders = null;
        [SerializeField] private SpriteRenderer _slimeSprite = null;
        [SerializeField] private float _detectionRadius;
        [SerializeField] private ContactFilter2D _contactFilter;
        private IAstarAI ai;
        public Transform Self => transform;
        public Transform Player {get; set;}
        private bool _canTick = true;
        private bool _isFacingRight;

        private void Awake()
        {
            ai = GetComponent<IAstarAI>();
            _damageInstigator.Init(_damageProperties);

            Dictionary<int, IState> states = new Dictionary<int, IState>();
            states.Add((int)EnemyStates.Detection, new CircleDetectionState(_stateController, this, _contactFilter, _detectionRadius));
            states.Add((int)EnemyStates.Follow, new GenericFollowState(_stateController, this, ai, _detectionRadius, _slimeSprite));

            _stateController.Init(states);
        }

        public void Init() {}

        private void Start() { _stateController.StartFromState((int)EnemyStates.Detection); }

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
            _slimeSprite.DOColor(Color.red, 0.3f).OnComplete(() =>
            { _slimeSprite.DOColor(Color.white, 0.3f); });
        }

        public void OnDead()
        {
            ai.destination = transform.position;
            _slimeSprite.transform.DOShakePosition(0.4f, 0.5f);
            _slimeSprite.DOFade(0f, 0.4f);

            foreach (var collider in _colliders)
            {
                collider.enabled = false;
            }
            _canTick = false;
        }

        public void OnRevive() {}
    }
}
