namespace Treasure.Interactables
{

    using Treasure.EventBus;
    using Treasure.Common;
    using Treasure.Swords;
    using UnityEngine;

    public class Sword : MonoBehaviour
    {
        [SerializeField] private ObjectId _requiredCharacter;
        [SerializeField] private ObjectId _itemId;
        [SerializeField] private SpriteRenderer _swordSprite = null;

        private void Start()
        {
            _swordSprite.sprite = SwordFactory.Instance.GetSwordById(_itemId.Value).SwordImage;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                if (col.GetComponent<IPlayableCharacter>().CharacterId.Value != _requiredCharacter.Value) return;

                EventBus<ConfirmAddSwordItem>.Raise(new ConfirmAddSwordItem
                {
                    itemId = _itemId.Value,
                    swordObject = gameObject

                });
            }
        }
    }
}