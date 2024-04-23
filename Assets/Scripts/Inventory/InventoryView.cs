﻿namespace Treasure.Inventory
{
    using UnityEngine;
    using DG.Tweening;
    using Treasure.Swords;
    using Treasure.Common;
    using Treasure.Inventory.Potions;

    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup = null;
        [SerializeField] private SwordInfoBox _currentSwordInfoBox = null;
        [SerializeField] private CanvasGroup[] _availableKeyItems;
        [SerializeField] private PotionContainer[] _potionContainers = null;
        [SerializeField] private InventoryController _inventoryController = null;

        public void ToggleVisibility(bool toggle)
        {
            Time.timeScale = toggle ? 0f : 1f;
            _canvasGroup.DOFade(toggle ? 1 : 0, 0.3f).SetUpdate(true);
            _canvasGroup.interactable = toggle;
            _canvasGroup.blocksRaycasts = toggle;

            if (toggle) UpdateInventory();
        }

        private void UpdateInventory()
        {
            SwordData equippedSword = SwordFactory.Instance.GetSwordById(_inventoryController.EquippedSword);
            _currentSwordInfoBox.Fill(equippedSword.SwordId.Value, equippedSword.SwordImage, equippedSword.Damage);

            bool[] keys = _inventoryController.GetAllKeyValues();
            for (int i = 0; i < _availableKeyItems.Length; i++)
            {
                _availableKeyItems[i].alpha = keys[i] ? 1f : 0.3f;
                _availableKeyItems[i].blocksRaycasts = keys[i];
            }           

            foreach (var container in _potionContainers)
                container.Init(false, null);

            DataProperty[] potions = _inventoryController.GetAllPotionValues();
            for (int i = 0; i < potions.Length; i++)
            {
                PotionData potionData = PotionFactory.Instance.GetPotionById(potions[i].propertyId.Value);
                if(potionData != null)
                {
                    _potionContainers[i].Init(true, potionData.PotionImage);
                    continue;
                }
            }
        }
    }
}