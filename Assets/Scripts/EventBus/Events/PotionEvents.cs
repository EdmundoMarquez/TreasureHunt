using UnityEngine;
using Treasure.EventBus;
using Treasure.Common;

public struct ThrowPotionItem : IEvent 
{
    public string potionId;
}

public struct OnPotionSelectCharacter : IEvent
{
    public string selectedCharacterId;
    public string potionId;
}
