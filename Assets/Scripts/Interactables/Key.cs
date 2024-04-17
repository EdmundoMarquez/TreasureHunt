namespace Treasure.Interactables
{
    using Treasure.EventBus;
    using Treasure.Common;
    using Treasure.Player;
    using UnityEngine;

    public class Key : MonoBehaviour
    {
        [SerializeField] private ObjectId _requiredCharacter;
        [SerializeField] private ObjectId _itemId;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                if (col.GetComponent<IPlayableCharacter>().CharacterId.Value != _requiredCharacter.Value) return;

                EventBus<AddKeyItem>.Raise(new AddKeyItem
                {
                    itemId = _itemId.Value
                });

                gameObject.SetActive(false);
            }
        }
    }
}
