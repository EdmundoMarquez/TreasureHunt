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
        private float _cooldownTime = 1f;
        private float _cooldownTimer;

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
            _swordPivot.DOLocalRotate(new Vector3(0, 0, -360), 0.5f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear).OnComplete(() =>
            {
                _cooldownTime = _cooldownTimer;
                _canTick = true;
            });
            _damageInstigator.ToggleInstigator(true, 0.5f);
            
        }

        public void FixedTick(){}

        public void Tick() 
        {
            if(!_canTick) return;
            
            _cooldownTimer -= Time.deltaTime;
            if(_cooldownTimer <= 0f) 
            {
                _stateController.ChangeToNextState((int)EnemyStates.Detection);
                Stop();
            }
        }

        public void Stop()
        {
            _canTick = false;
        }
    }
}