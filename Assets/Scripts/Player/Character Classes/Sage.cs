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
        [SerializeField] private SpriteRenderer _arrow = null;

        public ObjectId CharacterId => _characterId;
        private IPlayerInput _inputAdapter;
        private bool _canTick = false;
        public bool IsActive => _canTick;


        public void Init(IPlayerInput inputAdapter)
        {
            _healthController.Init(_characterAttributes.Health);
            _movementController.Init(_characterAttributes.Stamina);
            _followController.Init(_characterAttributes.Stamina);
            _potionController.Init(_characterId.Value);
            _healthBar.Init();

            _inputAdapter = inputAdapter;
            _interactionController.Init(_inputAdapter);
        }

        public void ToggleControl(bool toggle)
        {
            _movementController.Toggle(toggle);
            _healthController.Toggle(toggle);
            _interactionController.Toggle(toggle);
            _followController.Toggle(!toggle);
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
            _interactionController.Tick();
        }
    }
}