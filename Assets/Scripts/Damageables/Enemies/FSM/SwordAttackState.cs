namespace Treasure.Damageables
{
    using UnityEngine;
    using Treasure.Common;
    using Pathfinding;
    using DG.Tweening;
    public class SwordAttackState : IState
    {
        private StateController _stateController = null;
        private IEnemy _enemy;
        private DamageInstigator _damageInstigator = null;
        private Transform _swordPivot = null;
        private bool _canTick;
        private float _attackTime = 0.5f;
        private float _attackTimer;

        public SwordAttackState(StateController stateController, IEnemy enemy, DamageInstigator damageInstigator, Transform swordPivot)
        {
            _stateController = stateController;
            _enemy = enemy;
            _damageInstigator = damageInstigator;
            _swordPivot = swordPivot;
        }

        public void Awake()
        {
            
        }

        public void Init()
        {
            _canTick = true;

            _swordPivot.DOLocalRotate(new Vector3(0, 0, -360), _attackTime, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
            _damageInstigator.ToggleInstigator(true, _attackTime);
            _attackTime = _attackTimer;
        }

        public void FixedTick(){}

        public void Tick() 
        {
            if(!_canTick) return;
            
            _attackTimer -= Time.deltaTime;
            if(_attackTimer <= 0f) 
            {
                _stateController.DelayChangeToNextState((int)EnemyStates.Detection, 3);
                Stop();
            }
        }

        public void Stop()
        {
            _canTick = false;
        }
    }
}