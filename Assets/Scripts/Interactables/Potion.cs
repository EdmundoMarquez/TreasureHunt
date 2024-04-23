namespace Treasure.Interactables
{

    using Treasure.EventBus;
    using Treasure.Common;
    using Treasure.Inventory.Potions;
    using UnityEngine;

    public class Potion : MonoBehaviour
    {
        [SerializeField] private ObjectId _requiredCharacter;
        [SerializeField] private ObjectId _itemId;
        [SerializeField] private SpriteRenderer _potionSprite = null;
        private PotionData _potionData;

        private void Start()
        {
            _potionData = PotionFactory.Instance.GetPotionById(_itemId.Value);
            _potionSprite.sprite = _potionData.PotionImage;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                if (col.GetComponent<IPlayableCharacter>().CharacterId.Value != _requiredCharacter.Value) return;

                EventBus<AddPotionItem>.Raise(new AddPotionItem
                {
                    potionProperties = _potionData.Properties,
                    potionObject = gameObject
                });

                // gameObject.SetActive(false);
            }
        }
    }
}