namespace Treasure.Inventory
{

    using System.Collections.Generic;
    using System.Linq;
    using Treasure.EventBus;
    using Treasure.Common;
    using UnityEngine;

    public class InventoryController : MonoBehaviour, IEventReceiver<AddKeyItem>, IEventReceiver<AddSwordItem>, IEventReceiver<AddPotionItem>, IEventReceiver<RemovePotionItem>
    {
        [SerializeField] private KeyData[] _keys;
        [SerializeField] private ObjectId _startingSword = null;
        private Dictionary<string, bool> _idToKey;
        private List<DataProperty> _potionsInStorage;
        private string _equippedSword;
        public string EquippedSword => _equippedSword;
        private const int POTIONS_LIMIT_IN_INVENTORY = 3;
        public delegate void OnUpdateInventory();
        public OnUpdateInventory onUpdateInventory;

        private void Awake()
        {
            _idToKey = new Dictionary<string, bool>();

            foreach (var key in _keys)
            {
                _idToKey.Add(key.Id, key.IsUnlocked);
            }

            _potionsInStorage = new List<DataProperty>();

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

        public void OnEvent(AddPotionItem e)
        {
            if (_potionsInStorage.Count >= POTIONS_LIMIT_IN_INVENTORY)
            {
                Debug.Log("Exceeded max numbers of potions to store.");
                return;
            }

            _potionsInStorage.Add(e.potionProperties);
            e.potionObject.SetActive(false);
        }

        public void OnEvent(RemovePotionItem e)
        {
            var potionToRemove = _potionsInStorage.SingleOrDefault(p => p.propertyId.Value == e.potionId);
            if(potionToRemove == null) return;
            _potionsInStorage.Remove(potionToRemove);

            if(onUpdateInventory != null) onUpdateInventory();
        }

        public bool GetKeyById(string id)
        {
            if (!_idToKey.TryGetValue(id, out bool keyUnlocked)) return false;
            return keyUnlocked;
        }

        public bool[] GetAllKeyValues()
        {
            return _idToKey.Values.ToArray();
        }

        public string[] GetAllKeyIds()
        {
            return _idToKey.Keys.ToArray();
        }

        public DataProperty[] GetAllPotionValues()
        {
            return _potionsInStorage.ToArray();
        }

        private void OnEnable()
        {
            EventBus<AddKeyItem>.Register(this);
            EventBus<AddSwordItem>.Register(this);
            EventBus<AddPotionItem>.Register(this);
            EventBus<RemovePotionItem>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<AddKeyItem>.UnRegister(this);
            EventBus<AddSwordItem>.UnRegister(this);
            EventBus<RemovePotionItem>.UnRegister(this);
        }
    }
}