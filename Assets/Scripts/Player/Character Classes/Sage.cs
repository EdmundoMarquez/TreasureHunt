namespace Treasure.Player
{
    using UnityEngine;
    using Treasure.Common;

    public class Sage : MonoBehaviour, IPlayableCharacter
    {
        [SerializeField] private ObjectId _characterId = null;
        [SerializeField] private CharacterAttributes _characterAttributes = null;
        [SerializeField] private MovementController _movementController = null;
        [SerializeField] private CharacterHealthController _healthController = null;
        [SerializeField] private CharacterHealthBar _healthBar = null;
        [SerializeField] private CharacterPotionController _potionController = null;
        public ObjectId CharacterId => _characterId;

        private void Start()
        {
            _healthController.Init(_characterAttributes.Health);
            _movementController.Init(_characterAttributes.Stamina);
            _potionController.Init(_characterId.Value);
            _healthBar.Init();
        }

        public void ToggleControl(bool toggle)
        {
            _movementController.Toggle(toggle);
        }
    }
}