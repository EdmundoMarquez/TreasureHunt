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
        private float _horizontalScale;
        public bool IsDead {get; set;}

        private void Awake()
        {
            ai = GetComponent<IAstarAI>();
            _horizontalScale = _spritesTransform.localScale.x;

            Dictionary<int, IState> states = new Dictionary<int, IState>();
            states.Add((int)EnemyStates.Detection, new CircleDetectionState(_stateController, this, _contactFilter, _detectionRadius));
            states.Add((int)EnemyStates.Follow, new TakeAimPositionState(_stateController, this, ai, _detectionRadius, _minDistanceToAim, _horizontalScale, _spritesTransform, _animator, _bowPivot));
            states.Add((int)EnemyStates.Aim, new BowAttackState(_stateController, this, _arrowPrefab, _bowPivot, _arrowSpawnPoint, _spritesTransform, _horizontalScale, _aimRotationSpeed, _minDistanceToAim));
            states.Add((int)EnemyStates.Dead, new DeadState(_goblinSprites, _colliders, false));
            _stateController.Init(states);
        }

        public void Init() { _stateController.StartFromState((int)EnemyStates.Detection); }

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
            if(IsDead) return;
            ai.destination = transform.position;

            _stateController.ChangeToNextState((int)EnemyStates.Dead);
            IsDead = true;
        }

        public void OnRevive() { }

        public void OnEvent(OnPlayerCharacterSwitch e)
        {
            if(IsDead || _stateController.CurrentStateIndex != (int)EnemyStates.Follow || _stateController.CurrentStateIndex != (int)EnemyStates.Aim) 
                return;

            Player = e.currentCharacter;
            _stateController.ChangeToNextState((int)EnemyStates.Follow);
        }
    }
}
