namespace Treasure.Common
{
    using Treasure.PlayerInput;
    using UnityEngine;

    public interface IPlayableCharacter
    {
        ObjectId CharacterId { get; }
        bool IsActive {get;}
        void ToggleControl(bool toggle);
        void SetCharacterSprite(bool show);
        bool IsFullHealth {get;}
        bool IsDead {get;}
    }
}