using UnityEngine;

namespace Treasure.PlayerInput
{
    public class UnityInputAdapter : IPlayerInput
    {
        private float horizontal;
        private float vertical;
        public Vector2 GetDirection()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            return new Vector2(horizontal, vertical);
        }

        public bool AttackButtonPressed()
        {
            return Input.GetKeyDown(KeyCode.Z);
        }

        public bool InteractButtonPressed()
        {
            return Input.GetKeyDown(KeyCode.Z);
        }

        public bool InventoryButtonPressed()
        {
            return Input.GetKeyDown(KeyCode.X);
        }

        public bool ChangeCharacterButtonPressed()
        {
            return Input.GetKeyDown(KeyCode.C);
        }
        public bool PauseButtonPressed()
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }
}