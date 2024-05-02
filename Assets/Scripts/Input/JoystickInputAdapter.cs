using UnityEngine;

namespace Treasure.PlayerInput
{
    public class JoystickInputAdapter : IPlayerInput
    {
        private readonly Joystick _joystick;
        //private readonly JoyButton _interactJoyButton;
        //private readonly JoyButton _takeJoyButton;

        /*public JoystickInputAdapter(Joystick joystick, JoyButton interactJoyButton, JoyButton takeJoyButton)
        {
            _joystick = joystick;
            _interactJoyButton = interactJoyButton;
            _takeJoyButton = takeJoyButton;
        }*/
        public JoystickInputAdapter(Joystick joystick)
        {
            _joystick = joystick;
            //_interactJoyButton = interactJoyButton;
            //_takeJoyButton = takeJoyButton;
        }
        public Vector2 GetDirection()
        {
            return new Vector2(_joystick.Horizontal, _joystick.Vertical);
        }
        public bool AttackButtonPressed()
        {
            return false;
        }

        public bool InteractButtonPressed()
        {
            return false;
        }

        public bool InventoryButtonPressed()
        {
            return false;
        }

        public bool ChangeCharacterButtonPressed()
        {
            return false;
        }
        public bool PauseButtonPressed()
        {
            return false;
        }
    }

}
