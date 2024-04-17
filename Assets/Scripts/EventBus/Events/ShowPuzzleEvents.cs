using UnityEngine;
using Treasure.EventBus;
using Treasure.Common;

public struct OnShowPuzzle : IEvent 
{
    public GameObject chest;
    public WordData puzzleWord;
}
