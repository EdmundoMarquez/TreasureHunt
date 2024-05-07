namespace Treasure.Common
{
    using Treasure.PlayerInput;
    using UnityEngine;

    public interface IPlayableCharacter
    {
        ObjectId CharacterId { get; }
        void Init(IPlayerInput inputAdapter);
        void ToggleControl(bool toggle);
        void ShowControlArrow(bool show);
    }
}