using UnityEngine;
using System.Collections;
using Treasure.EventBus;
using Treasure.Common;

public struct OnPlayerCharactersGenerated : IEvent
{
    public IPlayableCharacter[] characters;
}
public struct OnCompletedDungeon : IEvent {}
public struct OnPlayerCharacterDefeated : IEvent 
{
    public string damageInstigator;
}
public struct GameOverEvent : IEvent 
{
    public string instigatorId;
}
public struct OnChestGenerated : IEvent {}