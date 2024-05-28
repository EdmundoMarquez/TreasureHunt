namespace PCG
{
    using System;
    using UnityEngine;

    public abstract class PlacementData
    {
        [Min(0)]
        public int minQuantity = 0;
        [Min(0)]
        [Tooltip("Max is inclusive")]
        public int maxQuantity = 0;
        public int Quantity
            => UnityEngine.Random.Range(minQuantity, maxQuantity + 1);
    }

    [Serializable]
    public class ItemPlacementData : PlacementData
    {
        public ItemData itemData;
    }

    [Serializable]
    public class EnemyPlacementData : PlacementData
    {
        public GameObject enemyPrefab;
        public Vector2Int enemySize = Vector2Int.one;
    }

    [Serializable]
    public class TreasurePlacementData : PlacementData
    {
        public GameObject treasurePrefab;
        public Vector2Int treasureSize = Vector2Int.one;
    }

    [Serializable]
    public class ObjectivePlacementData : PlacementData
    {
        public GameObject objectivePrefab;
    }
}