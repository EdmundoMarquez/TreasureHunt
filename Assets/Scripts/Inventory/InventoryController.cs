namespace Treasure.Inventory
{

    using System.Collections.Generic;
    using System.Linq;
    using Treasure.EventBus;
    using Treasure.Common;
    using UnityEngine;

    public class InventoryController : MonoBehaviour, IEventReceiver<AddKeyItem>, IEventReceiver<AddSwordItem>
    {
        [SerializeField] private KeyData[] _keys;
        private Dictionary<string, bool> _idToKey;
        [SerializeField] private ObjectId _startingSword = null;
        private string _equippedSword;
        public string EquippedSword => _equippedSword;

        private void Awake()
        {
            _idToKey = new Dictionary<string, bool>();

            foreach (var key in _keys)
            {
                _idToKey.Add(key.Id, key.IsUnlocked);
            }

            _equippedSword = _startingSword.Value;
        }

        public void OnEvent(AddKeyItem e)
        {
            _idToKey[e.itemId] = true;
        }

        public void OnEvent(AddSwordItem e)
        {
            _equippedSword = e.newItemId;
        }

        public bool GetKeyById(string id)
        {
            if (!_idToKey.TryGetValue(id, out bool keyUnlocked)) return false;
            return keyUnlocked;
        }

        public string[] GetAllKeys()
        {
            return _idToKey.Keys.ToArray();
        }

        private void OnEnable()
        {
            EventBus<AddKeyItem>.Register(this);
            EventBus<AddSwordItem>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<AddKeyItem>.UnRegister(this);
            EventBus<AddSwordItem>.UnRegister(this);
        }
    }
}