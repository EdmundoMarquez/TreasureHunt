namespace Treasure.Player
{
    using Treasure.Common;
    using Treasure.Inventory.Potions;
    using Treasure.EventBus;

    using UnityEngine;
    
    public class CharacterPotionController : MonoBehaviour, IEventReceiver<OnPotionSelectCharacter> 
    {
        [SerializeField] private CharacterHealthController _healthController = null;
        [SerializeField] private PotionFactory _potionFactory = null;
        [SerializeField] private PotionDataConfiguration _potionDataConfiguration = null;
        private string _characterId;

        public void Init(string characterId)
        {
            _characterId = characterId;
        }

        public void OnEvent(OnPotionSelectCharacter e)
        {
            if(e.selectedCharacterId != _characterId) return;
            ApplyPotion(e.potionId);
        }

        private void ApplyPotion(string potionId)
        {
            foreach(var potion in _potionDataConfiguration.healingPotions)
            {
                if(potion.Properties.propertyId.Value == potionId)
                {
                    _healthController.Heal(potion.Properties.amount);
                    return;
                }
            }
        }

        private void OnEnable() 
        {
            EventBus<OnPotionSelectCharacter>.Register(this);
        }

        private void OnDisable() 
        {
            EventBus<OnPotionSelectCharacter>.UnRegister(this);
        }
    }
}