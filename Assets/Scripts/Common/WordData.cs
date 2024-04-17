namespace Treasure.Common
{
    using UnityEngine;
    using System;

    [CreateAssetMenu(fileName = "WordData", menuName = "Puzzle/WordData", order = 0)]
    public class WordData : ScriptableObject 
    {
        [SerializeField] private string _completeWord;
        [SerializeField] private LetterPosition[] _missingLetters;
        public string Word => _completeWord;
        public LetterPosition[] MissingLetters => _missingLetters;
    }

    [Serializable]
    public class LetterPosition
    {
        public char letter;
        public int position;
    }
}