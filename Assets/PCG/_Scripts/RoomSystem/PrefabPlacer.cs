namespace PCG
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using Treasure.Common;
    using Treasure.Damageables;

    public class PrefabPlacer : MonoBehaviour
    {
        [SerializeField]
        private GameObject itemPrefab;

        public GameObject PlaceObjective(ObjectivePlacementData objectivePlacementData, ItemPlacementHelper itemPlacementHelper)
        {
            GameObject placedObject = null;

            Vector2? possiblePlacementSpot = itemPlacementHelper.GetItemPlacementPosition(
                        PlacementType.OpenSpace,
                        100,
                        Vector2Int.one,
                        false
                        );
            if (possiblePlacementSpot.HasValue)
            {
                placedObject = CreateObject(objectivePlacementData.objectivePrefab, possiblePlacementSpot.Value + new Vector2(0.5f, 0.5f)); //Instantiate(placementData.enemyPrefab,possiblePlacementSpot.Value + new Vector2(0.5f, 0.5f), Quaternion.identity)
            }

            return placedObject;
        }

        public List<GameObject> PlaceEnemies(List<EnemyPlacementData> enemyPlacementData, ItemPlacementHelper itemPlacementHelper)
        {
            List<GameObject> placedObjects = new List<GameObject>();

            foreach (var placementData in enemyPlacementData)
            {
                for (int i = 0; i < placementData.Quantity; i++)
                {
                    Vector2? possiblePlacementSpot = itemPlacementHelper.GetItemPlacementPosition(
                        PlacementType.OpenSpace,
                        100,
                        placementData.enemySize,
                        false
                        );
                    if (possiblePlacementSpot.HasValue)
                    {
                        GameObject enemyObject = CreateObject(placementData.enemyPrefab, possiblePlacementSpot.Value + new Vector2(0.5f, 0.5f)); //Instantiate(placementData.enemyPrefab,possiblePlacementSpot.Value + new Vector2(0.5f, 0.5f), Quaternion.identity)
                        enemyObject.GetComponent<IEnemy>().Init();
                        placedObjects.Add(enemyObject);
                    }
                }
            }
            return placedObjects;
        }

        public List<GameObject> PlaceTreasures(List<TreasurePlacementData> treasurePlacementData, ItemPlacementHelper itemPlacementHelper)
        {
            List<GameObject> placedObjects = new List<GameObject>();

            foreach (var placementData in treasurePlacementData)
            {
                for (int i = 0; i < placementData.Quantity; i++)
                {
                    Vector2? possiblePlacementSpot = itemPlacementHelper.GetItemPlacementPosition(
                        PlacementType.OpenSpace,
                        100,
                        placementData.treasureSize,
                        false
                    );
                    if (possiblePlacementSpot.HasValue)
                    {
                        placedObjects.Add(CreateObject(placementData.treasurePrefab, possiblePlacementSpot.Value + new Vector2(0.5f, 0.5f)));
                    }
                }
            }
            return placedObjects;
        }

        public List<GameObject> PlaceAllItems(List<ItemPlacementData> itemPlacementData, ItemPlacementHelper itemPlacementHelper)
        {
            List<GameObject> placedObjects = new List<GameObject>();

            IEnumerable<ItemPlacementData> sortedList = new List<ItemPlacementData>(itemPlacementData).OrderByDescending(placementData => placementData.itemData.size.x * placementData.itemData.size.y);

            foreach (var placementData in sortedList)
            {
                for (int i = 0; i < placementData.Quantity; i++)
                {
                    Vector2? possiblePlacementSpot = itemPlacementHelper.GetItemPlacementPosition(
                        placementData.itemData.placementType,
                        100,
                        placementData.itemData.size,
                        placementData.itemData.addOffset);


                    if (possiblePlacementSpot.HasValue)
                    {
                        placedObjects.Add(PlaceItem(placementData.itemData, possiblePlacementSpot.Value));
                    }
                }
            }
            return placedObjects;
        }
        private GameObject PlaceItem(ItemData item, Vector2 placementPosition)
        {
            GameObject newItem = CreateObject(itemPrefab, placementPosition);
            //GameObject newItem = Instantiate(itemPrefab, placementPosition, Quaternion.identity);
            newItem.GetComponent<Item>().Initialize(item);
            return newItem;
        }

        public GameObject CreateObject(GameObject prefab, Vector3 placementPosition)
        {
            if (prefab == null)
                return null;
            GameObject newItem;
            if (Application.isPlaying)
            {
                newItem = Instantiate(prefab, placementPosition, Quaternion.identity);
            }
            else
            {
                newItem = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                newItem.transform.position = placementPosition;
                newItem.transform.rotation = Quaternion.identity;
            }

            return newItem;
        }
    }
}