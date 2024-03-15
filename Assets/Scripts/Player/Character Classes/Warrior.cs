using UnityEngine;

public class Warrior: MonoBehaviour, IPlayableCharacter
{
    [SerializeField] private ObjectId _characterId = null;
    [SerializeField] private CharacterHealthBar _healthBar = null;
    [SerializeField] private CharacterHealthController _healthController = null;
    [SerializeField] private MovementController _movementController = null;
    [SerializeField] private SwordAttackController _swordAttackController = null;
    public ObjectId CharacterId => _characterId;

    private void Start()
    {
        _healthController.Init();
        _healthBar.Init();
    }

    public void ToggleControl(bool toggle)
    {
        _swordAttackController.Toggle(toggle);
        _movementController.Toggle(toggle);
    }
}