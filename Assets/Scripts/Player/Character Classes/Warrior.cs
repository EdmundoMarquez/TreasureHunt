namespace Treasure.Player
{
    using Treasure.Common;
    using Treasure.PlayerInput;
    using UnityEngine;
    using DG.Tweening;
    using Treasure.Inventory;

    public class Warrior : MonoBehaviour, IPlayableCharacter
    {
        [SerializeField] private ObjectId _characterId = null;
        [SerializeField] private CharacterAttributes _characterAttributes = null;
        [SerializeField] private CharacterHealthBar _healthBar = null;
        [SerializeField] private CharacterHealthController _healthController = null;
        [SerializeField] private MovementController _movementController = null;
        [SerializeField] private SwordAttackController _swordAttackController = null;
        [SerializeField] private CharacterPotionController _potionController = null;
        [SerializeField] private CompanionFollowController _followController = null;
        [SerializeField] private SpriteRenderer _arrow = null;
        public ObjectId CharacterId => _characterId;
        private IPlayerInput _inputAdapter;
        private bool _canTick = false;
        public bool IsActive => _canTick;

        public void Init(IPlayerInput inputAdapter, string swordId)
        {
            _healthController.Init(_characterAttributes.Health);
            _movementController.Init(_characterAttributes.Stamina);
            _followController.Init(_characterAttributes.Stamina);
            _potionController.Init(_characterId.Value);
            _swordAttackController.Init(swordId);
            _healthBar.Init();

            _inputAdapter = inputAdapter;
        }
        
        public void ToggleControl(bool toggle)
        {
            _swordAttackController.Toggle(toggle);
            _followController.Toggle(!toggle);
            _movementController.Toggle(toggle);
            _healthController.Toggle(toggle);
            _canTick = toggle;

            ShowControlArrow(toggle);
        }

        public void ShowControlArrow(bool show)
        {
            _arrow.DOFade(show ? 1f : 0f, 0.3f);
        }

        public void Tick()
        {
            if(!_canTick) 
            {
                _followController.Follow();
                return;
            }

            _movementController.Move(_inputAdapter.GetDirection());

            if(_inputAdapter.AttackButtonPressed())
            {
                _swordAttackController.Attack();
            }
        }

    }
}