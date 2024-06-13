using UnityEngine;
using Treasure.EventBus;
using Treasure.Common;

public struct OnShowPuzzle : IEvent 
{
    public GameObject chest;
    public WordData puzzleWord;
    public int currentTries;
}

public struct OnFailPuzzleTry : IEvent{}

public struct OnOpenChest : IEvent
{
    public Transform itemParent;
    public Vector3 itemPlacementPosition;
}

public struct OnGainReward : IEvent
{
    public int coinAmount;
}
