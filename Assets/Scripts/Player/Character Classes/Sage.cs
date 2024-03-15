using UnityEngine;

public class Sage: MonoBehaviour, IPlayableCharacter
{
    [SerializeField] private ObjectId _characterId = null;
    [SerializeField] private MovementController _movementController = null;
    [SerializeField] private CharacterHealthController _healthController = null;
    [SerializeField] private CharacterHealthBar _healthBar = null;
    public ObjectId CharacterId => _characterId;

    private void Start()
    {
        _healthController.Init();
        _healthBar.Init();
    }

    public void ToggleControl(bool toggle)
    {
        _movementController.Toggle(toggle);
    }
}