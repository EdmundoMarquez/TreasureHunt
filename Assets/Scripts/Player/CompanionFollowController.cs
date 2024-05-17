namespace Treasure.Player
{
    using UnityEngine;
    using Pathfinding;

    public class CompanionFollowController : MonoBehaviour
    {
        [SerializeField] private Transform _characterToFollow = null;
        [SerializeField] private Transform _spriteTransform = null;
        [SerializeField] private Animator _spriteAnimator = null;
        [SerializeField] private float _minDistanceToRush = 20f;
        [SerializeField] private float _rushMultiplier = 1.5f;
        private float _moveSpeed;
        private float _horizontalScale;
        private bool _canMove;
        private bool _isFacingRight = true;
        private AIPath _aiPath;

        private void Awake()
        {
            _aiPath = GetComponent<AIPath>();
            _horizontalScale = _spriteTransform.localScale.x;
        }

        public void Init(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
        }

        public void Toggle(bool toggle)
        {
            _canMove = toggle;
            _spriteAnimator.SetFloat("speed", 0f);
            _aiPath.canMove = toggle;
            _aiPath.destination = transform.position;
        }

        public void Follow()
        {
            if (!_canMove) return;

            float distance = Vector3.Distance(transform.position, _characterToFollow.position);
            _aiPath.maxSpeed = distance > _minDistanceToRush ? _moveSpeed * _rushMultiplier : _moveSpeed;

            float magnitude = _aiPath.velocity.magnitude;
            _spriteAnimator.SetFloat("speed", magnitude);

            if (magnitude <= 0f)
            {
                _spriteTransform.localScale = new Vector3(
                    _isFacingRight ? _horizontalScale : -_horizontalScale,
                    _spriteTransform.localScale.y,
                    _spriteTransform.localScale.z
                );
            }

            _isFacingRight = (_characterToFollow.position - transform.position).x > 0;

            _spriteTransform.localScale = new Vector3(
                _isFacingRight ? _horizontalScale : -_horizontalScale,
                _spriteTransform.localScale.y,
                _spriteTransform.localScale.z
            );

            _aiPath.destination = _characterToFollow.position;
        }
    }

}