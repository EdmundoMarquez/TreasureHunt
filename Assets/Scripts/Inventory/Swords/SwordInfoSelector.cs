namespace Treasure.Inventory
{
    using UnityEngine;
    using UnityEngine.UI;
    using Treasure.EventBus;
    using TMPro;
    using System.Collections.Generic;

    public class SwordInfoSelector : MonoBehaviour
    {
        [SerializeField] private InventoryController _inventoryController = null;
        [SerializeField] private Button _equipButton = null;
        [SerializeField] private Button _trashButton = null;
        [SerializeField] private Button _leftButton = null;
        [SerializeField] private Button _rightButton = null;
        [SerializeField] private SwordInfoBox _swordInfoBox = null;
        [SerializeField] private GameObject _equippedIcon = null;
        [SerializeField] private TMP_Text _swordIndexText = null;
        private List<SwordInventoryData> swordsInStorage;
        private string equippedSwordId;
        private int currentSelectorIndex = 0;

        public void Start()
        {
            _leftButton.onClick.AddListener(() => OnChangeIndex(-1));
            _rightButton.onClick.AddListener(() => OnChangeIndex(1));
            _equipButton.onClick.AddListener(() => EquipSword(currentSelectorIndex));
            _trashButton.onClick.AddListener(() => DiscardSword(currentSelectorIndex));
        }

        private void EquipSword(int index)
        {
            _inventoryController.EquippedSword = swordsInStorage[currentSelectorIndex].SwordId;
            _equippedIcon.SetActive(true);

            EventBus<EquipSwordItem>.Raise(new EquipSwordItem
            {
                swordId = swordsInStorage[currentSelectorIndex].SwordId
            });
        }

        private void DiscardSword(int index)
        {
            if(swordsInStorage.Count < 1) return;

            EventBus<RemoveSwordItem>.Raise(new RemoveSwordItem
            {
                swordId = swordsInStorage[currentSelectorIndex].SwordId
            });

            currentSelectorIndex = 0;
            EquipSword(currentSelectorIndex);
            UpdateSelector();
        }

        private void OnChangeIndex(int amount)
        {
            currentSelectorIndex += amount;

            _swordInfoBox.Fill(swordsInStorage[currentSelectorIndex].SwordId, swordsInStorage[currentSelectorIndex].SwordImage, swordsInStorage[currentSelectorIndex].Damage);
            _equippedIcon.SetActive(swordsInStorage[currentSelectorIndex].SwordId == _inventoryController.EquippedSword);
            _swordIndexText.SetText($"{currentSelectorIndex + 1} / 3");

            RefreshSelectorButtons();
        }

        private void RefreshSelectorButtons()
        {
            _leftButton.interactable = swordsInStorage.Count > 1  && currentSelectorIndex >= 1;
            _rightButton.interactable = swordsInStorage.Count > 1 && currentSelectorIndex < swordsInStorage.Count - 1;
            _trashButton.interactable = swordsInStorage.Count > 1;
        }

        public void UpdateSelector()
        {
            swordsInStorage = new List<SwordInventoryData>();
            var swords = _inventoryController.GetAllSwords();

            foreach (var sword in swords)
                for (int i = 0; i < sword.Quantity; i++)
                    swordsInStorage.Add(sword);
            
            for (int i = 0; i < swordsInStorage.Count; i++)
            {
                if(swordsInStorage[i].SwordId == _inventoryController.EquippedSword)
                {
                    _swordInfoBox.Fill(swordsInStorage[i].SwordId, swordsInStorage[i].SwordImage, swordsInStorage[i].Damage);
                    _equippedIcon.SetActive(true);
                    _swordIndexText.SetText($"{currentSelectorIndex + 1} / 3");
                    currentSelectorIndex = i;
                }    
            }

            RefreshSelectorButtons();
        }
    }

}