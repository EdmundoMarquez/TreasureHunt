using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Treasure.Common;
using Treasure.EventBus;
using Treasure.Inventory.Potions;

public class Item : MonoBehaviour
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

    private bool isPickable;
    private ObjectId pickableId;
    private PickableTypes pickableType;
    private ObjectId characterThatCanPickId;

    public UnityEvent OnGetHit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
        gameObject.layer = LayerMask.NameToLayer("Obstacles");
    }

    private void SetAsPickable(ItemData itemData)
    {
        itemCollider.isTrigger = true;

        isPickable = itemData.isPickable;
        pickableId = itemData.pickableId;
        pickableType = itemData.pickableType;
        characterThatCanPickId = itemData.characterThatCanPickId;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (col.GetComponent<IPlayableCharacter>().CharacterId.Value != characterThatCanPickId.Value) return;


            switch (pickableType)
            {
                case PickableTypes.Key:
                    EventBus<AddKeyItem>.Raise(new AddKeyItem { itemId = pickableId.Value});
                    gameObject.SetActive(false);
                    break;
                case PickableTypes.Potion:
                    PotionData data = PotionFactory.Instance.GetPotionById(pickableId.Value);
                    EventBus<AddPotionItem>.Raise(new AddPotionItem {
                          potionObject = gameObject,
                          potionProperties = data.Properties
                        });
                    break;
                case PickableTypes.Sword:
                    EventBus<ConfirmAddSwordItem>.Raise(new ConfirmAddSwordItem { 
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

    public void GetHit(int damage, GameObject damageDealer)
    {
        if (nonDestructible)
            return;
        if (health > 1)
            Instantiate(hitFeedback, spriteRenderer.transform.position, Quaternion.identity);
        else
            Instantiate(destoyFeedback, spriteRenderer.transform.position, Quaternion.identity);
        spriteRenderer.transform.DOShakePosition(0.2f, 0.3f, 75, 1, false, true).OnComplete(ReduceHealth);
    }

    private void ReduceHealth()
    {
        health--;
        if (health <= 0)
        {
            spriteRenderer.transform.DOComplete();
            Destroy(gameObject);
        }

    }
}

