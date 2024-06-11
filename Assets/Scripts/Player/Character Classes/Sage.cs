namespace Treasure.Player
{
    using UnityEngine;
    using Treasure.Common;
    using Treasure.PlayerInput;
    using DG.Tweening;

    public class Sage : MonoBehaviour, IPlayableCharacter
    {
        [SerializeField] private ObjectId _characterId = null;
        [SerializeField] private CharacterAttributes _characterAttributes = null;
        [SerializeField] private MovementController _movementController = null;
        [SerializeField] private CharacterHealthController _healthController = null;
        [SerializeField] private CharacterHealthBar _healthBar = null;
        [SerializeField] private CharacterPotionController _potionController = null;
        [SerializeField] private CharacterInteractionController _interactionController = null;
        [SerializeField] private CompanionFollowController _followController = null;
        [SerializeField] private CharacterMessageBox _messageBox = null;
        [SerializeField] private SpriteRenderer _characterSprite = null;
        [SerializeField] private GameObject _blurMinimapVolume = null;
        public ObjectId CharacterId => _characterId;
        private IPlayerInput _inputAdapter;
        private bool _canTick = false;
        public bool IsActive => _canTick;
        public bool IsFullHealth => _healthController.Health >= _healthController.MaxHealth;
        public bool IsDead => _healthController.Health <= 0;

        public void Init(IPlayerInput inputAdapter)
        {
            _healthController.Init(_characterAttributes.Health, _characterAttributes.MaxHealth);
            _movementController.Init(_characterAttributes.Speed);
            _followController.Init(_characterAttributes.Speed);
            _potionController.Init(_characterId.Value, _movementController, _followController, _healthController);
            _healthBar.Init();
            _messageBox.Init(this);

            _inputAdapter = inputAdapter;
            _interactionController.Init(_inputAdapter);
        }

        public void ToggleControl(bool toggle)
        {
            _movementController.Toggle(toggle);
            _healthController.Toggle(toggle);
            _interactionController.Toggle(toggle);
            _blurMinimapVolume.SetActive(toggle);
            _followController.Toggle(!toggle);
            _canTick = toggle;

            SetCharacterSprite(toggle);
        }

        public void SetCharacterSprite(bool toggle)
        {
            //Set character outline
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

            _characterSprite.GetPropertyBlock(propertyBlock);

            propertyBlock.SetInt("_Intensity", toggle ? 1 : 0);
            _characterSprite.SetPropertyBlock(propertyBlock);

            _characterSprite.sortingOrder = toggle ? 3 : 0;
        }

        public void Tick()
        {
            if(IsDead) return;
            
            if(!_canTick)
            {
                _followController.Follow();
                return;
            } 

            _movementController.Move(_inputAdapter.GetDirection());
            _interactionController.Tick();
        }

        private void OnDisable()
        {
            //Save health
            _characterAttributes.Health = _healthController.Health;
        }
    }
}