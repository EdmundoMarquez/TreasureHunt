namespace Treasure.Damageables
{
    using UnityEngine;
    using Treasure.Common;
    using Pathfinding;
    public class GenericFollowState : IState
    {
        private StateController _stateController = null;
        private IEnemy _enemy;
        private float _detectionRadius;
        private bool _canTick;
        private IAstarAI _ai;
        private SpriteRenderer _sprite = null;
        private bool _isFacingRight;
        public GenericFollowState(StateController stateController, IEnemy enemy, IAstarAI ai, float detectionRadius, SpriteRenderer sprite)
        {
            _stateController = stateController;
            _enemy = enemy;
            _ai = ai;
            _detectionRadius = detectionRadius;
            _sprite = sprite;
        }

        public void Awake(){}

        public void Init()
        {
            _canTick = true;
            Debug.Log("Entered follow state");
        }

        public void Tick()
        {
            if(!_canTick) return;
            SetSpriteOrientation();

            if (Vector3.Distance(_enemy.Self.position, _enemy.Player.position) > _detectionRadius * 1.5f)
            {
                _ai.destination = _enemy.Self.position;
                _stateController.DelayChangeToNextState((int)EnemyStates.Detection, 1);
                Stop();
            }
            else _ai.destination = _enemy.Player.position;
        }

        public void FixedTick(){}

        public void Stop()
        {
            _canTick = false;
        }

        private void SetSpriteOrientation()
        {
            Vector3 targetVector = _enemy.Player.position - _enemy.Self.position;

            float magnitude = targetVector.magnitude;

            if (magnitude <= 0f)
            {
                _sprite.flipX = _isFacingRight;
                return;
            }
            _isFacingRight = targetVector.x < 0;
            _sprite.flipX = _isFacingRight;
        }
    }
}