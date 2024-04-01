using UnityEngine;
using Treasure.EventBus;

public struct OnShowPuzzle : IEvent 
{
    public WordData puzzleWord;
}
