using UnityEngine;

namespace Treasure.InfiniteMode
{
    [CreateAssetMenu(fileName = "DungeonLevelData", menuName = "Infinite/DungeonLevelData", order = 1)]
    public class DungeonLevelData : ScriptableObject
    {
        public int currentLevel = 1;
    }
}