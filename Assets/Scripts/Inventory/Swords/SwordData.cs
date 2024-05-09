﻿namespace Treasure.Swords
{
    using UnityEngine;
    using Treasure.Common;

    [CreateAssetMenu(fileName = "SwordData", menuName = "Items/SwordData", order = 0)]
    public class SwordData : ScriptableObject
    {
        public ObjectId SwordId;
        public Sprite SwordImage;
        public DataProperty[] Damage;
    }
}