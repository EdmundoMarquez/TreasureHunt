using Treasure.Common;
using UnityEngine;
namespace Treasure.EventBus
{
    public struct OnDungeonFloorReady : IEvent 
    {
        public DungeonData dungeonData;
    }
}