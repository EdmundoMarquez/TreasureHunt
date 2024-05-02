namespace Treasure.Common
{
    using Treasure.PlayerInput;

    public interface IPlayableCharacter
    {
        ObjectId CharacterId { get; }
        void Init(IPlayerInput inputAdapter);
        void ToggleControl(bool toggle);
    }
}