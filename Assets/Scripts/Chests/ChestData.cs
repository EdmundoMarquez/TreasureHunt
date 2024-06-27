namespace Treasure.Chests
{
    using UnityEngine;
    using Treasure.Common;

    [System.Serializable]
    public class ChestData
    {
        public WordData WordData;
        public TrapType trapType = TrapType.None;
        public int Tries;
    }
}