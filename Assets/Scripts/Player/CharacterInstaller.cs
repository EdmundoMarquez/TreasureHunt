namespace Treasure.Player
{
    using Cinemachine;
    using UnityEngine;
    using Treasure.PlayerInput;
    using Treasure.Inventory;

    public class CharacterInstaller : MonoBehaviour
    {

        [SerializeField] private CinemachineVirtualCamera _followCamera = null;
        [SerializeField] private Warrior _warriorCharacter = null;
        [SerializeField] private Sage _sageCharacter = null;
        [SerializeField] private InventoryView _inventoryView = null;
        private IPlayerInput _inputAdapter;
        private bool isWarriorActive = true;


        private void Awake()
        {
            isWarriorActive = true;
            _inputAdapter = new UnityInputAdapter();
        }

        private void Start()
        {
            _warriorCharacter.Init(_inputAdapter);
            _sageCharacter.Init(_inputAdapter);
            _inventoryView.Init(_inputAdapter);

            //Set warrior as the default
            _warriorCharacter.ToggleControl(true);
            _sageCharacter.ToggleControl(false);
        }

        private void Update()
        {
            _warriorCharacter.Tick();
            _sageCharacter.Tick();
            _inventoryView.Tick();

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