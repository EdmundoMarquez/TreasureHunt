﻿namespace Treasure.Player
{
    using UnityEngine;

    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private Transform _spriteTransform = null;
        [SerializeField] private Animator _spriteAnimator = null;
        private float _horizontalScale;
        private bool _canMove;
        private bool _isFacingRight = true;

        private void Awake()
        {
            _horizontalScale = _spriteTransform.localScale.x;
        }

        public void Toggle(bool toggle)
        {
            _canMove = toggle;
            _spriteAnimator.SetFloat("speed", 0f);
        }

        void Update()
        {
            if (!_canMove) return;

            Vector2 movementVector = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))).normalized;

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