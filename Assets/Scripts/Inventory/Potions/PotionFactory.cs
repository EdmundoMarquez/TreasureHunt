namespace Treasure.Inventory
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PotionFactory : MonoBehaviour
    {
        [SerializeField] private ItemDataConfiguration _itemDataConfiguration = null;
        private Dictionary<string, PotionData> _idToPotion;
        public static PotionFactory Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            _idToPotion = new Dictionary<string, PotionData>();

            foreach (var potion in _itemDataConfiguration.GetAllPotions())
            {
                _idToPotion.Add(potion.Properties.propertyId.Value, potion);
            }
        }

        public PotionData GetPotionById(string id)
        {
            if (!_idToPotion.TryGetValue(id, out var potion)) { return null; }
            return potion;
        }

    }
}