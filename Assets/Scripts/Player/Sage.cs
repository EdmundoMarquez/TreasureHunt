using UnityEngine;

public class Sage: MonoBehaviour, IPlayableCharacter
{
    [SerializeField] private ObjectId _characterId = null;
    [SerializeField] private MovementController _movementController = null;
    public ObjectId CharacterId => _characterId;

    public void ToggleControl(bool toggle)
    {
        _movementController.Toggle(toggle);
    }
}