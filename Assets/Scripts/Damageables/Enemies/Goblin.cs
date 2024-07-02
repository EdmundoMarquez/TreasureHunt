namespace Treasure.Damageables
{

    using UnityEngine;
    using DG.Tweening;
    using Treasure.Common;
    using Treasure.EventBus;
    using Pathfinding;
    using System.Collections.Generic;

    public class Goblin : MonoBehaviour, IEnemy, IEventReceiver<OnPlayerCharacterSwitch>
    {
        [SerializeField] private DamageableHealthController _healthController = null;
        [SerializeField] private StateController _stateController = null;
        [SerializeField] private DataProperty[] _damageProperties;
        [SerializeField] private DamageInstigator _damageInstigator = null;
        [SerializeField] private Animator _animator = null;
        [SerializeField] private Transform _swordPivot = null;
        [SerializeField] private Collider2D[] _colliders = null;
        [SerializeField] private SpriteRenderer _goblinSprite = null;
        [SerializeField] private Transform _spritesTransform = null;
        [SerializeField] private float _detectionRadius;
        [SerializeField] private ContactFilter2D _contactFilter;
        private IAstarAI ai;
        public Transform Self => transform;
        public Transform Player {get; set;}
        private bool _canTick = true;
        private bool _isFacingRight;
        private float _horizontalScale;

        private void Awake()
        {
            ai = GetComponent<IAstarAI>();
            _horizontalScale = _spritesTransform.localScale.x;
            _damageInstigator.Init(_damageProperties);

            Dictionary<int, IState> states = new Dictionary<int, IState>();
            states.Add((int)EnemyStates.Detection, new CircleDetectionState(_stateController, this, _contactFilter, _detectionRadius));
            states.Add((int)EnemyStates.Follow, new AggroFollowState(_stateController, this, ai, _detectionRadius, _horizontalScale, _spritesTransform, _animator));
            states.Add((int)EnemyStates.Attack, new SwordAttackState(_stateController, this, _damageInstigator, _swordPivot));

            _stateController.Init(states);
        }

        public void Init() {}

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
            // _batSprite.transform.DOShakePosition(0.4f, 0.5f);
            _goblinSprite.DOColor(Color.red, 0.3f).OnComplete(() =>
            { _goblinSprite.DOColor(Color.white, 0.3f); });
        }

        public void OnDead()
        {
            ai.destination = transform.position;
            _goblinSprite.transform.DOShakePosition(0.4f, 0.5f);
            _goblinSprite.DOFade(0f, 0.4f);

            foreach (var collider in _colliders)
            {
                collider.enabled = false;
            }
            _canTick = false;
        }

        public void OnRevive() {}

        public void OnEvent(OnPlayerCharacterSwitch e)
        {
            Player = e.currentCharacter;
            _stateController.ChangeToNextState((int)EnemyStates.Follow);
        }
    }
}
