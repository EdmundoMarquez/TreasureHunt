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
        [SerializeField] private SpriteRenderer[] _goblinSprites = null;
        [SerializeField] private Transform _spritesTransform = null;
        [SerializeField] private float _detectionRadius;
        [SerializeField] private ContactFilter2D _contactFilter;
        [SerializeField] private int _maxCoinsToSteal = 500;
        [SerializeField] private bool _stealCoinsOnHit = false;
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
            _damageInstigator.Init(_damageProperties);

            Dictionary<int, IState> states = new Dictionary<int, IState>();
            states.Add((int)EnemyStates.Detection, new CircleDetectionState(_stateController, this, _contactFilter, _detectionRadius));
            states.Add((int)EnemyStates.Follow, new AggroFollowState(_stateController, this, ai, _detectionRadius, _horizontalScale, _spritesTransform, _animator));
            states.Add((int)EnemyStates.Attack, new SwordAttackState(_stateController, this, _damageInstigator, _swordPivot));

            _stateController.Init(states);
        }

        public void Init() { }

        private void Start() 
        { 
            _stateController.StartFromState((int)EnemyStates.Detection); 
            if(_stealCoinsOnHit) _damageInstigator.onHitDamageable += () => 
            { 
                EventBus<LoseCoinsEvent>.Raise(new LoseCoinsEvent
                {
                    coinsAmount = Random.Range(0, _maxCoinsToSteal)
                }); 
            };
        }


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
            _stateController.StopStates();
        }

        public void OnRevive() { }

        public void OnEvent(OnPlayerCharacterSwitch e)
        {
            if(_stateController.CurrentStateIndex != (int)EnemyStates.Follow
            || _stateController.CurrentStateIndex != (int)EnemyStates.Attack) return;
            Player = e.currentCharacter;
            _stateController.ChangeToNextState((int)EnemyStates.Follow);
        }
    }
}
