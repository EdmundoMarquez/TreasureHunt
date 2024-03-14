using UnityEngine;
namespace Treasure.EventBus
{
    public struct AddKeyItem : IEvent 
    {
        public string itemId;
    }
}
