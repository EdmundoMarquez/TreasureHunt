namespace Treasure.Player
{
    using System.Collections;
    using UnityEngine;

    public class MovementController : MonoBehaviour
    {
        [SerializeField] private Transform _spriteTransform = null;
        [SerializeField] private Animator _spriteAnimator = null;
        private float _moveSpeed;
        private float _horizontalScale;
        private bool _canMove;
        private bool _isFacingRight = true;
        private Coroutine ChangeSpeedCoroutine;

        private void Awake()
        {
            _horizontalScale = _spriteTransform.localScale.x;
        }

        public void Init(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
        }

        public void ChangeSpeed(float duration, float amount = 2)
        {
            if(ChangeSpeedCoroutine != null) StopCoroutine(ChangeSpeedCoroutine);
            StartCoroutine(ChangeSpeed_Timer(_moveSpeed + amount, duration));
        }

        private IEnumerator ChangeSpeed_Timer(float temporarySpeed, float duration)
        {
            Debug.Log("Start change speed");
            float previousSpeed = _moveSpeed;
            _moveSpeed = temporarySpeed;
            yield return new WaitForSeconds(duration);
            _moveSpeed = previousSpeed;
            Debug.Log("End change speed");
        }

        public void Toggle(bool toggle)
        {
            _canMove = toggle;
            _spriteAnimator.SetFloat("speed", 0f);
        }

        public void Move(Vector2 movementVector)
        {
            if (!_canMove) return;

            float magnitude = movementVector.magnitude;
            _spriteAnimator.SetFloat("speed", magnitude);

            if (magnitude <= 0f)
            {
                _spriteTransform.localScale = new Vector3(
                    _isFacingRight ? _horizontalScale : -_horizontalScale,
                    _spriteTransform.localScale.y,
                    _spriteTransform.localScale.z
                );
                return;
            }

            _isFacingRight = movementVector.x > 0;

            _spriteTransform.localScale = new Vector3(
                _isFacingRight ? _horizontalScale : -_horizontalScale,
                _spriteTransform.localScale.y,
                _spriteTransform.localScale.z
            );

            Vector2 targetPosition = transform.position;
            targetPosition.x += movementVector.x;
            targetPosition.y += movementVector.y;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
        }
    }

}