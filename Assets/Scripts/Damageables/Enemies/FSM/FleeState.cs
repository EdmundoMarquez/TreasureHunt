
namespace Treasure.Damageables
{
    using UnityEngine;
    using Treasure.Common;
    using Pathfinding;
    public class FleeState : IState
    {
        private StateController _stateController = null;
        private IEnemy _enemy;
        private bool _canTick;
        private IAstarAI _ai;
        private Transform _sprite = null;
        private bool _isFacingRight;
        private Vector2 _randomFleePoint;
        private Animator _animator;
        private float _horizontalScale;
        public FleeState(StateController stateController, IEnemy enemy, IAstarAI ai, Transform sprite, Animator animator, float horizontalScale)
        {
            _stateController = stateController;
            _enemy = enemy;
            _ai = ai;
            _sprite = sprite;
            _animator = animator;
            _horizontalScale = horizontalScale;
        }

        public void Awake(){}

        public void Init()
        {
            _randomFleePoint = PickRandomPoint();
            _ai.destination = _randomFleePoint;
            _ai.SearchPath();
            _animator.speed = 2f;
            _ai.maxSpeed = 1f;
            _canTick = true;
        }

        public void Tick()
        {
            if(!_canTick) return;
            
            if(Vector2.Distance(_enemy.Self.position, _randomFleePoint) < 2f)
            {
                if(_enemy.IsDead) return;
                _stateController.ChangeToNextState((int)EnemyStates.Follow);
            }

            SetSpriteOrientation();
        }

        public void FixedTick(){}

        public void Stop()
        {
            _canTick = false;
            _animator.speed = 1f;
            _ai.maxSpeed = 0.7f;
        }

        private Vector2 PickRandomPoint()
        {
            Vector2 randomPoint = (Vector2)_ai.position + Random.insideUnitCircle * 10f;

            while(Vector2.Distance(_enemy.Self.position, randomPoint) < 5f)
            {
                randomPoint = (Vector2)_ai.position + Random.insideUnitCircle * 10f;
            }

            return randomPoint;
        }

        private void SetSpriteOrientation()
        {
            float distance = Vector3.Distance(_randomFleePoint, _enemy.Player.position);

            float magnitude = _ai.velocity.magnitude;
            _animator.SetFloat("speed", magnitude);

            if (magnitude <= 0f)
            {
                _sprite.transform.localScale = new Vector3(
                    _isFacingRight ? _horizontalScale : -_horizontalScale,
                    _sprite.transform.localScale.y,
                    _sprite.transform.localScale.z
                );
            }

            _isFacingRight = (_randomFleePoint - (Vector2)_enemy.Self.position).x > 0;

            _sprite.transform.localScale = new Vector3(
                _isFacingRight ? _horizontalScale : -_horizontalScale,
                _sprite.transform.localScale.y,
                _sprite.transform.localScale.z
            );
        }
    }
}