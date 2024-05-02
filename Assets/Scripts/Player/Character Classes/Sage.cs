namespace Treasure.Player
{
    using UnityEngine;
    using Treasure.Common;
    using Treasure.PlayerInput;
    using Pathfinding;

    public class Sage : MonoBehaviour, IPlayableCharacter
    {
        [SerializeField] private ObjectId _characterId = null;
        [SerializeField] private CharacterAttributes _characterAttributes = null;
        [SerializeField] private MovementController _movementController = null;
        [SerializeField] private CharacterHealthController _healthController = null;
        [SerializeField] private CharacterHealthBar _healthBar = null;
        [SerializeField] private CharacterPotionController _potionController = null;
        [SerializeField] private Transform _followCharacter = null;
        private AIPath _aiPath;
        public ObjectId CharacterId => _characterId;
        private IPlayerInput _inputAdapter;
        private bool _canTick = false;

        private void Start()
        {
            _aiPath = GetComponent<AIPath>();
        }

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
            _movementController.Toggle(toggle);
            _aiPath.enabled = !toggle; //Enable only when no active
            _canTick = toggle;
        }

        public void Tick()
        {
            if(!_canTick)
            {
                _aiPath.destination = _followCharacter.position;
                return;
            } 

            _movementController.Move(_inputAdapter.GetDirection());

            if(_inputAdapter.InteractButtonPressed())
            {
                //...Interact
            }
        }
    }
}