using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Treasure.EventBus;
using UnityEngine;

public class InventoryController : MonoBehaviour, IEventReceiver<AddKeyItem>
{
    [SerializeField] private KeyData[] _keys;
    private Dictionary<string, bool> _idToKey;

    private void Awake()
    {
        _idToKey = new Dictionary<string, bool>();

        foreach (var key in _keys)
        {
            _idToKey.Add(key.Id, key.IsUnlocked);
        }
    }

    public void OnEvent(AddKeyItem e)
    {
        _idToKey[e.itemId] = true;
    }

    public bool GetKeyById(string id)
    {
        if(!_idToKey.TryGetValue(id, out bool keyUnlocked)) return false;
        return keyUnlocked;
    }

    public string[] GetAllKeys()
    {
        return _idToKey.Keys.ToArray();
    }

    private void OnEnable()
    {
        EventBus<AddKeyItem>.Register(this);
    }

    private void OnDisable()
    {
        EventBus<AddKeyItem>.UnRegister(this);
    }
}
