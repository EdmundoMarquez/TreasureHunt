namespace Treasure.Damageables
{
    using UnityEngine;
    using Treasure.Common;
    using Pathfinding;
    public class CircleDetectionState : IState
    {
        private StateController _stateController = null;
        private IEnemy _enemy;
        private ContactFilter2D _contactFilter;
        private float _detectionRadius;
        private bool _canTick;

        public CircleDetectionState(StateController stateController, IEnemy enemy, ContactFilter2D contactFilter, float detectionRadius)
        {
            _stateController = stateController;
            _enemy = enemy;
            _contactFilter = contactFilter;
            _detectionRadius = detectionRadius;
        }

        public void Awake()
        {
            
        }

        public void Init()
        {
            _canTick = true;
            Debug.Log("Entered detection state");
        }

        public void FixedTick()
        {
            if(!_canTick) return;
            if(DetectPlayerInRange()) _stateController.ChangeToNextState((int)EnemyStates.Follow);
        }

        public void Tick() {}

        public void Stop()
        {
            _canTick = false;
        }

        private bool DetectPlayerInRange()
        {
            Collider2D[] results = new Collider2D[8];
            int hits = Physics2D.OverlapCircle(_enemy.Self.position, _detectionRadius, _contactFilter, results);

            if (hits > 0)
            {
                IPlayableCharacter character = results[0].GetComponent<IPlayableCharacter>();
                if (!character.IsActive) return false;
                _enemy.Player = results[0].transform;
                return true;
            }
            return _enemy.Player != null;
        }
    }
}