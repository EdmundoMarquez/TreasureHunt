using UnityEngine;
using System.Collections;
using Treasure.EventBus;
using Treasure.Common;

public struct OnPlayerCharacterDefeated : IEvent 
{
    public string damageInstigator;
}

public struct CharacterRequiredMessageEvent : IEvent
{
    public string characterId;
}

public struct InventoryFullMessageEvent : IEvent{}