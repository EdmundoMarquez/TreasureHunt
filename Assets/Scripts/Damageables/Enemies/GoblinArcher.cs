namespace Treasure.Damageables
{

    using UnityEngine;
    using DG.Tweening;
    using Treasure.Common;
    using Treasure.EventBus;
    using Pathfinding;
    using System.Collections.Generic;

    public class GoblinArcher : MonoBehaviour, IEnemy, IEventReceiver<OnPlayerCharacterSwitch>
    {
        [SerializeField] private DamageableHealthController _healthController = null;
        [SerializeField] private StateController _stateController = null;
        [SerializeField] private Arrow _arrowPrefab = null;
        [SerializeField] private Animator _animator = null;
        [SerializeField] private Transform _bowPivot = null;
        [SerializeField] private Transform _arrowSpawnPoint = null;
        [SerializeField] private Collider2D[] _colliders = null;
        [SerializeField] private SpriteRenderer[] _goblinSprites = null;
        [SerializeField] private Transform _spritesTransform = null;
        [SerializeField] private float _detectionRadius;
        [SerializeField] private float _minDistanceToAim;
        [SerializeField] private float _aimRotationSpeed = 5f;
        [SerializeField] private ContactFilter2D _contactFilter;
        private IAstarAI ai;
        public Transform Self => transform;
        public Transform Player { get; set; }
        private bool _canTick = true;
        private bool _isFacingRight;
        private float _horizontalScale;

        private void Awake()
        {
            ai = GetComponent<IAstarAI>();
            _horizontalScale = _spritesTransform.localScale.x;

            Dictionary<int, IState> states = new Dictionary<int, IState>();
            states.Add((int)EnemyStates.Detection, new CircleDetectionState(_stateController, this, _contactFilter, _detectionRadius));
            states.Add((int)EnemyStates.Follow, new TakeAimPositionState(_stateController, this, ai, _detectionRadius, _minDistanceToAim, _horizontalScale, _spritesTransform, _animator, _bowPivot));
            states.Add((int)EnemyStates.Aim, new BowAttackState(_stateController, this, _arrowPrefab, _bowPivot, _arrowSpawnPoint, _spritesTransform, _horizontalScale, _aimRotationSpeed, _minDistanceToAim));
            _stateController.Init(states);
        }

        public void Init() { }

        private void Start() { _stateController.StartFromState((int)EnemyStates.Detection); }

        private void OnEnable()
        {
            EventBus<OnPlayerCharacterSwitch>.Register(this);
            _healthController.onDestroyDamageable += OnDead;
            _healthController.onDamageFeedback += OnDamage;
        }

        private void OnDisable()
        {
            EventBus<OnPlayerCharacterSwitch>.UnRegister(this);
            _healthController.onDestroyDamageable -= OnDead;
            _healthController.onDamageFeedback -= OnDamage;
        }

        public void OnDamage()
        {
            foreach (var sprite in _goblinSprites)
            {
                sprite.DOColor(Color.red, 0.3f).OnComplete(() =>
                { sprite.DOColor(Color.white, 0.3f); });

            }
        }

        public void OnDead()
        {
            ai.destination = transform.position;

            foreach (var sprite in _goblinSprites)
            {
                sprite.transform.DOShakePosition(0.4f, 0.5f);
                sprite.DOFade(0f, 0.4f);
            }

            foreach (var collider in _colliders)
            {
                collider.enabled = false;
            }
            _canTick = false;
        }

        public void OnRevive() { }

        public void OnEvent(OnPlayerCharacterSwitch e)
        {
            Player = e.currentCharacter;
            _stateController.ChangeToNextState((int)EnemyStates.Follow);
        }
    }
}
