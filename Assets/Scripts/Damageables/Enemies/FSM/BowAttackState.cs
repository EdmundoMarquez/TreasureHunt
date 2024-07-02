namespace Treasure.Damageables
{
    using UnityEngine;
    using Treasure.Common;
    using Pathfinding;
    using DG.Tweening;
    public class BowAttackState : IState
    {
        private StateController _stateController = null;
        private IEnemy _enemy;
        private Arrow _arrowPrefab = null;
        private Transform _bowPivot = null;
        private Transform _spawnPoint = null;
        private Transform _sprite = null;
        private bool _canTick;
        private bool _isFacingRight;
        private float _shootWaitTime = 3f;
        private float _shootWaitTimer;
        private float _horizontalScale;
        private float _rotationSpeed = 3f;
        private float _aimMinDistance;
        private Vector3 _bowInitialScale;
        private Vector3 _bowInitialRotation;
        private Vector3 _aimDirection;

        public BowAttackState(StateController stateController, IEnemy enemy, Arrow arrowPrefab, Transform bowPivot, Transform spawnPoint, Transform sprite, 
        float horizontalScale, float rotationSpeed, float aimMinDistance)
        {
            _stateController = stateController;
            _enemy = enemy;
            _bowPivot = bowPivot;
            _spawnPoint = spawnPoint;
            _sprite = sprite;
            _horizontalScale = horizontalScale;
            _rotationSpeed =rotationSpeed;
            _aimMinDistance = aimMinDistance;
            _arrowPrefab = arrowPrefab;
        }

        public void Awake()
        {

        }

        public void Init()
        {
            _canTick = true;

            Vector3 targetDirection = _enemy.Player.position - _enemy.Self.position;
            _isFacingRight = (targetDirection).x > 0;

            if(!_isFacingRight)
                _bowPivot.localScale = new Vector3(-_bowPivot.localScale.x, _bowPivot.localScale.y, _bowPivot.localScale.z);

            _bowInitialScale = _bowPivot.localScale;
            _shootWaitTimer = _shootWaitTime;
        }

        public void FixedTick() { }

        public void Tick()
        {
            if (!_canTick) return;

            if(Vector3.Distance(_enemy.Self.position, _enemy.Player.position) >= _aimMinDistance)
                _stateController.ChangeToNextState((int)EnemyStates.Follow);

            _shootWaitTimer -= Time.deltaTime;
            if(_shootWaitTimer <= 0f) 
            {
                Arrow arrow = Object.Instantiate(_arrowPrefab, _spawnPoint);
                arrow.transform.position = _spawnPoint.position;
                arrow.transform.parent = null;
                arrow.transform.localScale = Vector3.one * 0.5f;

                arrow.Init(_enemy.Player.position);

                _shootWaitTimer = _shootWaitTime;
                return;
            }

            SetSpritesOrientation();
        }

        public void Stop()
        {
            _canTick = false;
            _bowPivot.rotation = Quaternion.Euler(Vector3.zero);
            _bowPivot.localScale = _bowInitialScale;
        }

        private void SetSpritesOrientation()
        {
            _aimDirection = _enemy.Player.position - _enemy.Self.position;
            _isFacingRight = (_aimDirection).x > 0;

            if(_aimDirection == Vector3.zero) return;

            float angle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;
            _bowPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}