namespace Treasure.Player
{
    using Treasure.Common;
    using UnityEngine;

    public class Warrior : MonoBehaviour, IPlayableCharacter
    {
        [SerializeField] private ObjectId _characterId = null;
        [SerializeField] private CharacterAttributes _characterAttributes = null;
        [SerializeField] private CharacterHealthBar _healthBar = null;
        [SerializeField] private CharacterHealthController _healthController = null;
        [SerializeField] private MovementController _movementController = null;
        [SerializeField] private SwordAttackController _swordAttackController = null;
        [SerializeField] private CharacterPotionController _potionController = null;
        [SerializeField] private Collider2D _collider2D = null;
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
            _swordAttackController.Toggle(toggle);
            _movementController.Toggle(toggle);
            _collider2D.enabled = toggle;
        }
    }
}