namespace PCG
{
    using Treasure.Common;
    using UnityEngine;

    [CreateAssetMenu]
    public class ItemData : ScriptableObject
    {
        public Sprite sprite;
        public Vector2Int size = new Vector2Int(1, 1);
        public PlacementType placementType;
        public bool addOffset;
        public int health = 1;
        public bool nonDestructible;
        public bool isPickable;
        public ObjectId pickableId;
        public PickableTypes pickableType;
        public ObjectId characterThatCanPickId;
    }

    public enum PickableTypes
    {
        None,
        Key,
        Potion,
        Sword
    }

}