namespace Treasure.Inventory
{

    using System.Collections.Generic;
    using System.Linq;
    using Treasure.EventBus;
    using Treasure.Common;
    using UnityEngine;
    using System;

    public class InventoryController : MonoBehaviour, IEventReceiver<AddKeyItem>, IEventReceiver<AddSwordItem>, IEventReceiver<AddPotionItem>,
    IEventReceiver<RemoveSwordItem> ,IEventReceiver<RemovePotionItem>
    {
        [SerializeField] private InventoryData _inventoryData;
        [SerializeField] private ItemDataConfiguration _items;
        private Dictionary<string, KeyInventoryData> _idToKey;
        private Dictionary<string, PotionInventoryData> _potionsInStorage;
        private Dictionary<string, SwordInventoryData> _swordsInStorage;
        private ObjectId _equippedSword;
        public string EquippedSword {get; set;}
        private const int POTIONS_LIMIT_IN_INVENTORY = 3;
        private const int SWORDS_LIMIT_IN_INVENTORY = 3;
        public delegate void OnUpdateInventory();
        public OnUpdateInventory onUpdateInventory;

        private void Awake()
        {
            _idToKey = new Dictionary<string, KeyInventoryData>();
            _potionsInStorage = new Dictionary<string, PotionInventoryData>();
            _swordsInStorage = new Dictionary<string, SwordInventoryData>();

            foreach (var key in _inventoryData.Keys)
            {
                KeyInventoryData keyData = new KeyInventoryData();
                keyData.Id = key.Id;
                keyData.Unlocked = key.IsUnlocked;
                _idToKey.Add(key.Id, keyData);
            }

            foreach (var potion in _items.GetAllPotions())
                AddPotionToStorage(potion);

            foreach (var sword in _items.Swords)
                AddSwordToStorage(sword);

            LoadPersistentData();
            SaveInventory();
        }


        private void LoadPersistentData()
        {
            if (_inventoryData.EquippedSword != null)
                EquippedSword = _inventoryData.EquippedSword;

            foreach (var sword in _inventoryData.StoredSwords)
            {
                SwordInventoryData swordData = GetSwordById(sword.SwordId);
                if (swordData == null) continue;
                swordData.SwordId = sword.SwordId;
                swordData.SwordImage = sword.SwordImage;
                swordData.Damage = sword.Damage;
                swordData.Quantity = sword.Quantity;
            }

            foreach (var potion in _inventoryData.StoredPotions)
            {
                PotionInventoryData potionData = GetPotionById(potion.Properties.propertyId.Value);
                if (potionData == null) continue;
                potionData.PotionImage = potion.PotionImage;
                potionData.Properties = potion.Properties;
                potionData.Quantity = potion.Quantity;
            }

            //Store default sword 
            if (GetSwordsCount() < 1)
                GetSwordById(EquippedSword).Quantity += 1;

            EventBus<EquipSwordItem>.Raise(new EquipSwordItem
            {
                swordId = EquippedSword
            });
        }
        private void AddSwordToStorage(SwordData sword)
        {
            SwordInventoryData inventoryData = new SwordInventoryData();
            inventoryData.SwordId = sword.SwordId.Value;
            inventoryData.SwordImage = sword.SwordImage;
            inventoryData.Damage = sword.Damage;
            inventoryData.Quantity = 0;

            _swordsInStorage.Add(sword.SwordId.Value, inventoryData);
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
            _idToKey[e.itemId].Unlocked = true;
        }

        public void OnEvent(AddSwordItem e)
        {
            if (IsSwordSpaceFull()) return;

            if (_swordsInStorage.TryGetValue(e.itemId, out var inventoryData))
            {
                if(inventoryData.Quantity > 0)
                {
                    EventBus<InventoryFullMessageEvent>.Raise(new InventoryFullMessageEvent{});
                    return;
                } 
                inventoryData.Quantity += 1;
            }
            e.swordObject.SetActive(false);
        }

        public void OnEvent(AddPotionItem e)
        {
            if (IsPotionSpaceFull()) return;

            if (_potionsInStorage.TryGetValue(e.potionProperties.propertyId.Value, out var inventoryData))
            {
                inventoryData.Quantity += 1;
            }
            e.potionObject.SetActive(false);
        }

        public int GetSwordsCount()
        {
            int numberOfSwords = 0;

            foreach (var sword in GetAllSwords())
                numberOfSwords += sword.Quantity;

            return numberOfSwords;
        }

        public bool IsSwordSpaceFull()
        {
            if (GetSwordsCount() >= SWORDS_LIMIT_IN_INVENTORY)
            {
                EventBus<InventoryFullMessageEvent>.Raise(new InventoryFullMessageEvent{});
                return true;
            }
            return false;
        }

        public bool IsPotionSpaceFull()
        {
            int numberOfPotions = 0;

            foreach (var potion in GetAllPotions())
            {
                numberOfPotions += potion.Quantity;
                if (numberOfPotions >= POTIONS_LIMIT_IN_INVENTORY)
                {
                    EventBus<InventoryFullMessageEvent>.Raise(new InventoryFullMessageEvent{});
                    return true;
                }
            }
            return false;
        }

        public void OnEvent(RemoveSwordItem e)
        {
            if (!_swordsInStorage.TryGetValue(e.swordId, out var inventoryData))
            {
                return;
            }
            inventoryData.Quantity = Math.Abs(--inventoryData.Quantity);
            if (onUpdateInventory != null) onUpdateInventory();
        }

        public void OnEvent(RemovePotionItem e)
        {
            if (!_potionsInStorage.TryGetValue(e.potionId, out var inventoryData))
            {
                return;
            }
            inventoryData.Quantity = Math.Abs(--inventoryData.Quantity);
            if (onUpdateInventory != null) onUpdateInventory();
        }

        public KeyInventoryData GetKeyById(string id)
        {
            if (!_idToKey.TryGetValue(id, out var keyUnlocked)) return null;
            return keyUnlocked;
        }

        public SwordInventoryData GetSwordById(string id)
        {
            if (!_swordsInStorage.TryGetValue(id, out var sword)) return null;
            return sword;
        }

        public PotionInventoryData GetPotionById(string id)
        {
            if (!_potionsInStorage.TryGetValue(id, out var potion)) return null;
            return potion;
        }

        public KeyInventoryData[] GetAllKeys() { return _idToKey.Values.ToArray(); }
        public PotionInventoryData[] GetAllPotions() { return _potionsInStorage.Values.ToArray(); }
        public SwordInventoryData[] GetAllSwords() { return _swordsInStorage.Values.ToArray(); }

        public List<DataProperty> GetAllAvailablePotions()
        {
            List<DataProperty> availablePotions = new List<DataProperty>();

            foreach (var potion in GetAllPotions())
            {
                if (potion.Quantity > 0)
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
            _inventoryData.EquippedSword = EquippedSword;

            for (int i = 0; i < _inventoryData.Keys.Length; i++)
            {
                _inventoryData.Keys[i].IsUnlocked = GetKeyById(_inventoryData.Keys[i].Id).Unlocked;
            }

            _inventoryData.StoredPotions = _potionsInStorage.Values.ToList();
            _inventoryData.StoredSwords = _swordsInStorage.Values.ToList();
        }

        private void OnEnable()
        {
            EventBus<AddKeyItem>.Register(this);
            EventBus<AddSwordItem>.Register(this);
            EventBus<RemoveSwordItem>.Register(this);
            EventBus<AddPotionItem>.Register(this);
            EventBus<RemovePotionItem>.Register(this);
        }

        private void OnDisable()
        {
            SaveInventory();

            EventBus<AddKeyItem>.UnRegister(this);
            EventBus<AddSwordItem>.UnRegister(this);
            EventBus<RemoveSwordItem>.UnRegister(this);
            EventBus<AddPotionItem>.UnRegister(this);
            EventBus<RemovePotionItem>.UnRegister(this);
        }
    }
}