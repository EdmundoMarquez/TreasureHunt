namespace PCG
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Treasure.Player;

    public class PlayerRoom : RoomGenerator
    {
        public GameObject player;

        public List<ItemPlacementData> itemData;

        [SerializeField]
        private PrefabPlacer prefabPlacer;

        public override List<GameObject> ProcessRoom(
            Vector2Int roomCenter,
            HashSet<Vector2Int> roomFloor,
            HashSet<Vector2Int> roomFloorNoCorridors)
        {

            ItemPlacementHelper itemPlacementHelper =
                new ItemPlacementHelper(roomFloor, roomFloorNoCorridors);

            List<GameObject> placedObjects =
                prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

            Vector2Int playerSpawnPoint = roomCenter;

            GameObject playerObject
                = prefabPlacer.CreateObject(player, playerSpawnPoint + new Vector2(0.5f, 0.5f));

            placedObjects.Add(playerObject);

            return placedObjects;
        }
    }
}