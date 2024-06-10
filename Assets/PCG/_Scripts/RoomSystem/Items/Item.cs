namespace PCG
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.Events;
    using Treasure.Common;
    using Treasure.EventBus;
    using Treasure.Inventory;
    using Pathfinding;

    public class Item : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private BoxCollider2D itemCollider;

        [SerializeField]
        int health = 3;
        [SerializeField]
        bool nonDestructible;

        [SerializeField]
        private GameObject hitFeedback, destoyFeedback;
        [SerializeField]
        private MinimapIconDisplay minimap;

        private bool isPickable;
        private ObjectId pickableId;
        private PickableTypes pickableType;
        private ObjectId characterThatCanPickId;

        public void Initialize(ItemData itemData)
        {
            //set sprite
            spriteRenderer.sprite = itemData.sprite;
            //set sprite offset
            spriteRenderer.transform.localPosition = new Vector2(0.5f * itemData.size.x, 0.5f * itemData.size.y);
            itemCollider.size = itemData.size;
            itemCollider.offset = spriteRenderer.transform.localPosition;

            if (itemData.nonDestructible)
                nonDestructible = true;

            this.health = itemData.health;

            if (itemData.isPickable)
            {
                SetAsPickable(itemData);
                return;
            }
            //Not pickables should be obstacles
            gameObject.layer = LayerMask.NameToLayer("Props");
            spriteRenderer.gameObject.layer = LayerMask.NameToLayer("Props");
        }

        private void SetAsPickable(ItemData itemData)
        {
            itemCollider.isTrigger = true;

            isPickable = itemData.isPickable;
            pickableId = itemData.pickableId;
            pickableType = itemData.pickableType;
            characterThatCanPickId = itemData.characterThatCanPickId;

            minimap.Init(itemData.sprite);

            gameObject.layer = LayerMask.NameToLayer("Pickables");
            spriteRenderer.gameObject.layer = LayerMask.NameToLayer("Pickables");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                IPlayableCharacter character = col.GetComponent<IPlayableCharacter>();

                if (characterThatCanPickId != null)
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
                        EventBus<AddSwordItem>.Raise(new AddSwordItem
                        {
                            itemId = pickableId.Value,
                            swordObject = gameObject
                        });
                        break;
                    case PickableTypes.Coins:
                        EventBus<OnGainReward>.Raise(new OnGainReward
                        {
                            coinAmount = Random.Range(5, 10)
                        });
                        gameObject.SetActive(false);
                        break;
                    case PickableTypes.None:
                    default:
                        break;
                }
            }
        }

        private void RecalculateObstacle()
        {
            var bounds = GetComponent<Collider2D>().bounds;
            // Expand the bounds along the Z axis
            bounds.Expand(Vector3.forward * 1000);
            var guo = new GraphUpdateObject(bounds);
            // change some settings on the object
            AstarPath.active.UpdateGraphs(guo);
        }

        public void Damage(int damage, string instigator = "")
        {
            if (nonDestructible)
                return;
            // if (health > 1)
            //     Instantiate(hitFeedback, spriteRenderer.transform.position, Quaternion.identity);
            // else
            //     Instantiate(destoyFeedback, spriteRenderer.transform.position, Quaternion.identity);
            spriteRenderer.transform.DOShakePosition(0.2f, 0.3f, 75, 1, false, true).OnComplete(ReduceHealth);
        }

        private void ReduceHealth()
        {
            health--;
            if (health <= 0)
            {
                spriteRenderer.transform.DOComplete();
                RecalculateObstacle();
                gameObject.SetActive(false);
            }

        }
    }
}