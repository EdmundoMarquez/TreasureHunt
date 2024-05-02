using UnityEngine;

namespace Treasure.PlayerInput
{
    public interface IPlayerInput 
    {
        Vector2 GetDirection();
        bool InteractButtonPressed();
        bool AttackButtonPressed();
        bool InventoryButtonPressed();
        bool ChangeCharacterButtonPressed();
        bool PauseButtonPressed();
    }

}
