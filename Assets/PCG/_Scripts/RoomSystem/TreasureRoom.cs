namespace PCG
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Treasure.Dungeon;
    using Treasure.Chests;
    using UnityEngine;

    public class TreasureRoom : RoomGenerator
    {
        [SerializeField] 
        private DungeonLevelData dungeonLevelData;
        [SerializeField]
        private PrefabPlacer prefabPlacer;

        public List<TreasurePlacementData> treasurePlacementData;
        public List<ItemPlacementData> itemData;

        public override List<GameObject> ProcessRoom(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridors)
        {
            ItemPlacementHelper itemPlacementHelper =
                new ItemPlacementHelper(roomFloor, roomFloorNoCorridors);

            List<GameObject> placedObjects =
                prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

            List<GameObject> placedChests = prefabPlacer.PlaceTreasures(treasurePlacementData, itemPlacementHelper);

            foreach (var chest in placedChests)
                ChestBuilder.Instance.BuildChestFromData(chest.GetComponent<Chest>(), dungeonLevelData.currentLevel);

            placedObjects.AddRange(placedChests);

            return placedObjects;
        }
    }
}