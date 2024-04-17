namespace Treasure.Puzzle
{
    using UnityEngine;
    using Treasure.Common;

    [CreateAssetMenu(fileName = "ChestData", menuName = "Puzzle/ChestData", order = 0)]
    public class ChestData : ScriptableObject
    {
        [SerializeField] private WordData[] _wordBank;

        public WordData RandomizeWord()
        {
            return _wordBank[Random.Range(0, _wordBank.Length)];
        }
    }
}