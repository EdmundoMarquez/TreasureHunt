namespace Treasure.Rewards
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.Events;
    using Treasure.Common;
    using Treasure.EventBus;
    using Treasure.Inventory;
    using Treasure.Inventory.Potions;

    public class RewardItem : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private BoxCollider2D itemCollider;

        [SerializeField]
        private MinimapIconDisplay minimap;
        private bool _isPickable;
        private ObjectId _pickableId;
        private PickableTypes _pickableType;
        private ObjectId _characterThatCanPickId;
        private ItemData _itemData;
        public Sprite Icon => spriteRenderer?.sprite;
        public string Id => _pickableId?.Value;
        public ItemData Data => _itemData;

        public void Initialize(ItemData itemData)
        {
            //set sprite
            spriteRenderer.sprite = itemData.sprite;
            //set sprite offset
            spriteRenderer.transform.localPosition = new Vector2(0.5f * itemData.size.x, 0.5f * itemData.size.y);
            itemCollider.size = itemData.size;
            itemCollider.offset = spriteRenderer.transform.localPosition;
            itemCollider.enabled = false;

            //Set pickable data
            _isPickable = itemData.isPickable;
            _pickableId = itemData.pickableId;
            _pickableType = itemData.pickableType;
            _characterThatCanPickId = itemData.characterThatCanPickId;
            
            minimap.Init(itemData.sprite);
            _itemData = itemData;
        }

        public void EnablePickable()
        {
            itemCollider.enabled = true;
            itemCollider.isTrigger = true;

            gameObject.layer = LayerMask.NameToLayer("Pickables");
            spriteRenderer.gameObject.layer = LayerMask.NameToLayer("Pickables");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                IPlayableCharacter character = col.GetComponent<IPlayableCharacter>();
                if (character.CharacterId.Value != _characterThatCanPickId.Value)
                {
                    EventBus<CharacterRequiredMessageEvent>.Raise(new CharacterRequiredMessageEvent
                    {
                        characterId = _characterThatCanPickId.Value
                    });
                    return;
                } 

                if (!character.IsActive) return;

                switch (_pickableType)
                {
                    case PickableTypes.Key:
                        EventBus<AddKeyItem>.Raise(new AddKeyItem { itemId = _pickableId.Value });
                        gameObject.SetActive(false);
                        break;
                    case PickableTypes.Potion:
                        PotionData data = PotionFactory.Instance.GetPotionById(_pickableId.Value);
                        EventBus<AddPotionItem>.Raise(new AddPotionItem
                        {
                            potionObject = gameObject,
                            potionProperties = data.Properties
                        });
                        break;
                    case PickableTypes.Sword:
                        EventBus<AddSwordItem>.Raise(new AddSwordItem
                        {
                            itemId = _pickableId.Value,
                            swordObject = gameObject
                        });
                        break;
                    case PickableTypes.None:
                    default:
                        break;
                }
            }
        }
    }
}