namespace Treasure.Player
{
    using Cinemachine;
    using UnityEngine;
    using Treasure.PlayerInput;
    using Treasure.Inventory;

    public class PlayerInstaller : MonoBehaviour
    {

        [SerializeField] private Warrior _warriorCharacter = null;
        [SerializeField] private Sage _sageCharacter = null;
        [SerializeField] private bool _initOnStart = false;
        [SerializeField] private CinemachineVirtualCamera _followCamera = null;
        [SerializeField] private InventoryController _inventoryController = null;
        private IPlayerInput _inputAdapter;
        private bool isWarriorActive = true;

        private void Awake()
        {
            isWarriorActive = true;
            _inputAdapter = new UnityInputAdapter();
        }

        private void Start()
        {
            if(!_initOnStart) return;
            Init(_followCamera, _inventoryController);
        }

        public void Init(CinemachineVirtualCamera followCamera, InventoryController inventoryController)
        {
            _followCamera = followCamera;
            _warriorCharacter.Init(_inputAdapter, inventoryController.EquippedSword);
            _sageCharacter.Init(_inputAdapter);
            SetDefaultCharacter();
        }

        private void SetDefaultCharacter()
        {
            //Set warrior as the default
            _warriorCharacter.ToggleControl(true);
            _sageCharacter.ToggleControl(false);
            _followCamera.m_Follow = _warriorCharacter.transform;
        }

        private void Update()
        {
            _warriorCharacter.Tick();
            _sageCharacter.Tick();

            if (_inputAdapter.ChangeCharacterButtonPressed())
            {
                isWarriorActive = !isWarriorActive;
                _warriorCharacter.ToggleControl(isWarriorActive);
                _sageCharacter.ToggleControl(!isWarriorActive);

                _followCamera.m_Follow = isWarriorActive ? _warriorCharacter.transform : _sageCharacter.transform;
            }
        }

    }

}