namespace Treasure.Damageables
{
    using UnityEngine;
    using Treasure.Common;
    using Pathfinding;
    public class AggroFollowState : IState
    {
        private StateController _stateController = null;
        private IEnemy _enemy;
        private float _detectionRadius;
        private bool _canTick;
        private IAstarAI _ai;
        private Transform _sprite = null;
        private bool _isFacingRight;
        private bool _takingPositionToAttack = false;
        private float _horizontalScale;
        private Animator _animator;
        public AggroFollowState(StateController stateController, IEnemy enemy, IAstarAI ai, float detectionRadius, float horizontalScale, Transform sprite, Animator animator)
        {
            _horizontalScale = horizontalScale;
            _stateController = stateController;
            _enemy = enemy;
            _ai = ai;
            _detectionRadius = detectionRadius;
            _sprite = sprite;
            _animator = animator;
        }

        public void Awake(){}        

        public void Init()
        {
            _canTick = true;
            _takingPositionToAttack = false;
        }

        public void Tick()
        {
            if(!_canTick) return;
            SetSpriteOrientation();
            float distance = Vector3.Distance(_enemy.Self.position, _enemy.Player.position);

            if (distance > _detectionRadius * 1.5f)
            {
                _stateController.ChangeToNextState((int)EnemyStates.Detection);
                return;
            }

            if(distance < 3f)
            {
                if(!_takingPositionToAttack)
                {
                    _ai.destination = _enemy.Player.position * Random.insideUnitCircle;
                    _takingPositionToAttack = true;
                }

                if(_ai.reachedDestination)
                {
                    _stateController.ChangeToNextState((int)EnemyStates.Attack);
                    return;
                }
            }

            _ai.destination = _enemy.Player.position;
        }

        public void FixedTick(){}

        public void Stop()
        {
            _animator.SetFloat("speed", 0f);
            _ai.destination = _enemy.Self.position;
            _canTick = false;
        }

        private void SetSpriteOrientation()
        {
            float distance = Vector3.Distance(_enemy.Self.position, _enemy.Player.position);

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

            _isFacingRight = (_enemy.Player.position - _enemy.Self.position).x > 0;

            _sprite.transform.localScale = new Vector3(
                _isFacingRight ? _horizontalScale : -_horizontalScale,
                _sprite.transform.localScale.y,
                _sprite.transform.localScale.z
            );
        }
    }
}