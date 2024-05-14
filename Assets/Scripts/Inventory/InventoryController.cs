namespace Treasure.Inventory
{

    using System.Collections.Generic;
    using System.Linq;
    using Treasure.EventBus;
    using Treasure.Common;
    using Treasure.Inventory.Potions;
    using UnityEngine;
    using System;

    public class InventoryController : MonoBehaviour, IEventReceiver<AddKeyItem>, IEventReceiver<AddSwordItem>, IEventReceiver<AddPotionItem>, IEventReceiver<RemovePotionItem>
    {
        [SerializeField] private InventoryData _inventoryData;
        [SerializeField] private PotionDataConfiguration _potions;
        private Dictionary<string, bool> _idToKey;
        private Dictionary<string, PotionInventoryData> _potionsInStorage;
        private ObjectId _equippedSword;
        public string EquippedSword => _equippedSword?.Value;
        private const int POTIONS_LIMIT_IN_INVENTORY = 3;
        public delegate void OnUpdateInventory();
        public OnUpdateInventory onUpdateInventory;

        private void Awake()
        {
            _idToKey = new Dictionary<string, bool>();
            _potionsInStorage = new Dictionary<string, PotionInventoryData>();

            foreach (var potion in _potions.healingPotions)
            {
                AddPotionToStorage(potion);
            }
            //...Remaining Potions
        }

        public void Init()
        {
            foreach (var key in _inventoryData.Keys)
            {
                _idToKey.Add(key.Id, key.IsUnlocked);
            }

            if(_inventoryData.EquippedSword != null)
                _equippedSword = _inventoryData.EquippedSword;

            if(_inventoryData.StoredPotions.Count > 1) return;

            foreach (var potion in _inventoryData.StoredPotions)
            {
                PotionInventoryData potionData = GetPotionById(potion.Properties.propertyId.Value);
                if(potionData == null) continue;
                potionData.PotionImage = potion.PotionImage;
                potionData.Properties = potion.Properties;
                potionData.Quantity = potion.Quantity;
            }
        }

        private void AddPotionToStorage(PotionData potion)
        {
            PotionInventoryData inventoryData = new PotionInventoryData();
            inventoryData.PotionImage = potion.PotionImage;
            inventoryData.Properties = potion.Properties;
            inventoryData.Quantity = 0;

            _potionsInStorage.Add(potion.Properties.propertyId.Value, inventoryData);
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
            int numberOfPotions = 0;
            foreach (var potion in GetAllPotions())
            {
                numberOfPotions += potion.Quantity;
                if(numberOfPotions >= POTIONS_LIMIT_IN_INVENTORY)
                {
                    Debug.Log("Exceeded max numbers of potions to store.");
                    return;
                }
            }

            if(_potionsInStorage.TryGetValue(e.potionProperties.propertyId.Value, out var inventoryData))
            {
                inventoryData.Quantity += 1;
            }
            e.potionObject.SetActive(false);
        }

        public void OnEvent(RemovePotionItem e)
        {
            if(!_potionsInStorage.TryGetValue(e.potionId, out var inventoryData))
            {
                return;
            }
            inventoryData.Quantity = Math.Abs(--inventoryData.Quantity);
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

        public PotionInventoryData GetPotionById(string id)
        {
            if (!_potionsInStorage.TryGetValue(id, out var potion)) return null;
            return potion;
        }

        public PotionInventoryData[] GetAllPotions()
        {
            return _potionsInStorage.Values.ToArray();
        }

        public List<DataProperty> GetAllAvailablePotions()
        {
            List<DataProperty> availablePotions = new List<DataProperty>();

            foreach (var potion in GetAllPotions())
            {
                if(potion.Quantity > 0)
                {
                    for (int i = 0; i < potion.Quantity; i++)
                    {
                        availablePotions.Add(potion.Properties);
                    }
                }
            }

            return availablePotions;
        }

        public void SaveInventory()
        {
            _inventoryData.EquippedSword = _equippedSword;
            
            for (int i = 0; i < _inventoryData.Keys.Length; i++)
            {
                _inventoryData.Keys[i].IsUnlocked = GetKeyById(_inventoryData.Keys[i].Id);
            }

            _inventoryData.StoredPotions = _potionsInStorage.Values.ToList();
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
            SaveInventory();

            EventBus<AddKeyItem>.UnRegister(this);
            EventBus<AddSwordItem>.UnRegister(this);
            EventBus<RemovePotionItem>.UnRegister(this);
        }
    }
}