namespace Treasure.Player
{
    using System;
    using Treasure.Common;
    using Treasure.PlayerInput;
    using UnityEngine;
    using UnityEngine.Experimental.PlayerLoop;

    public class Warrior : MonoBehaviour, IPlayableCharacter
    {
        [SerializeField] private ObjectId _characterId = null;
        [SerializeField] private CharacterAttributes _characterAttributes = null;
        [SerializeField] private CharacterHealthBar _healthBar = null;
        [SerializeField] private CharacterHealthController _healthController = null;
        [SerializeField] private MovementController _movementController = null;
        [SerializeField] private SwordAttackController _swordAttackController = null;
        [SerializeField] private CharacterPotionController _potionController = null;
        public ObjectId CharacterId => _characterId;
        private IPlayerInput _inputAdapter;
        private bool _canTick = false;

        public void Init(IPlayerInput inputAdapter)
        {
            _healthController.Init(_characterAttributes.Health);
            _movementController.Init(_characterAttributes.Stamina);
            _potionController.Init(_characterId.Value);
            _healthBar.Init();

            _inputAdapter = inputAdapter;
        }
        
        public void ToggleControl(bool toggle)
        {
            _swordAttackController.Toggle(toggle);
            _movementController.Toggle(toggle);
            _canTick = toggle;
        }

        public void Tick()
        {
            if(!_canTick) return;

            _movementController.Move(_inputAdapter.GetDirection());

            if(_inputAdapter.AttackButtonPressed())
            {
                _swordAttackController.Attack();
            }
        }

    }
}