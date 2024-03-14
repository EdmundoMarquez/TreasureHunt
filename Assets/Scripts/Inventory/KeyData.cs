using UnityEngine;

[CreateAssetMenu(fileName = "KeyData", menuName = "KeyData", order = 1)]
public class KeyData : ScriptableObject 
{
    [SerializeField] ObjectId _objectId;
    public string Id => _objectId.Value;
    public bool IsUnlocked;
}