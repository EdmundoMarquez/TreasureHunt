public interface IPlayableCharacter
{
    ObjectId CharacterId {get;}
    void ToggleControl(bool toggle);
}