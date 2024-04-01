using UnityEngine;
using Treasure.EventBus;

public struct OnShowPuzzle : IEvent 
{
    public GameObject chest;
    public WordData puzzleWord;
}
