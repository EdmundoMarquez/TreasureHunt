using UnityEngine;
using Treasure.EventBus;
using Treasure.Common;

public struct OnShowPuzzle : IEvent 
{
    public GameObject chest;
    public WordData puzzleWord;
}

public struct OnGainReward : IEvent
{
    public int coinAmount;
}
