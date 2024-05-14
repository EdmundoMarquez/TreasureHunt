using Treasure.Common;
using UnityEngine;
namespace Treasure.EventBus
{
    public struct AddKeyItem : IEvent 
    {
        public string itemId;
    }

    public struct AddSwordItem : IEvent
    {
        public string previousItemId;
        public ObjectId newItemId;
    }

    public struct ConfirmAddSwordItem : IEvent
    {
        public string itemId;
        public GameObject swordObject;
    }

    public struct AddPotionItem : IEvent
    {
        public DataProperty potionProperties;
        public GameObject potionObject;
    }

    public struct RemovePotionItem : IEvent
    {
        public string potionId;
    }
}