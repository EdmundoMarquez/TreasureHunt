namespace Treasure.Chests
{
    using UnityEngine;
    using Treasure.Common;

    [CreateAssetMenu(fileName = "ChestData", menuName = "Puzzle/ChestData", order = 0)]
    public class ChestData : ScriptableObject
    {
        [SerializeField] private WordData[] _wordBank;
        [Range(0, 3)]
        [SerializeField] private int _numberOfTries;
        public int Tries => _numberOfTries;

        public WordData RandomizeWord()
        {
            return _wordBank[Random.Range(0, _wordBank.Length)];
        }
    }
}