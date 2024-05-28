namespace PCG
{
    using Treasure.EventBus;

    public struct OnDungeonFloorReady : IEvent 
    {
        public DungeonData dungeonData;
    }
}