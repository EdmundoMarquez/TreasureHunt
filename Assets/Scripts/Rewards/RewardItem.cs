namespace Treasure.Rewards
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.Events;
    using Treasure.Common;
    using Treasure.EventBus;
    using Treasure.Inventory.Potions;

    public class RewardItem : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private BoxCollider2D itemCollider;

        [SerializeField]
        private MinimapIconDisplay minimap;
        private bool isPickable;
        private ObjectId pickableId;
        private PickableTypes pickableType;
        private ObjectId characterThatCanPickId;
        public Sprite Icon => spriteRenderer?.sprite;
        public string Id => pickableId?.Value;

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
            isPickable = itemData.isPickable;
            pickableId = itemData.pickableId;
            pickableType = itemData.pickableType;
            characterThatCanPickId = itemData.characterThatCanPickId;
            
            minimap.Init(itemData.sprite);
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
                if (character.CharacterId.Value != characterThatCanPickId.Value) return;
                if (!character.IsActive) return;

                switch (pickableType)
                {
                    case PickableTypes.Key:
                        EventBus<AddKeyItem>.Raise(new AddKeyItem { itemId = pickableId.Value });
                        gameObject.SetActive(false);
                        break;
                    case PickableTypes.Potion:
                        PotionData data = PotionFactory.Instance.GetPotionById(pickableId.Value);
                        EventBus<AddPotionItem>.Raise(new AddPotionItem
                        {
                            potionObject = gameObject,
                            potionProperties = data.Properties
                        });
                        break;
                    case PickableTypes.Sword:
                        EventBus<ConfirmAddSwordItem>.Raise(new ConfirmAddSwordItem
                        {
                            itemId = pickableId.Value,
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