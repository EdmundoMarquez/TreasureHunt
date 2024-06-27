namespace Treasure.Common
{
    using UnityEngine;
    using System;
    using Random = UnityEngine.Random;

    [CreateAssetMenu(fileName = "WordDataConfiguration", menuName = "Puzzle/WordDataConfiguration", order = 1)]
    public class WordDataConfiguration : ScriptableObject 
    {
        public WordData[] TutorialData;
        public WordData[] Level1Data;
        public WordData[] Level2Data;
        public WordData[] Level3Data;
        public WordData[] Level4Data;

        public WordData RandomizeWordData(int currentLevel)
        {
            WordData wordData = null;

            if(currentLevel >= 15)
                wordData = Level4Data[Random.Range(0, Level4Data.Length)];
            else if(currentLevel >= 12)
                wordData = Level3Data[Random.Range(0, Level3Data.Length)];
            else if(currentLevel >= 8)
                wordData = Level2Data[Random.Range(0, Level2Data.Length)];
            else if(currentLevel >= 4)
                wordData = Level1Data[Random.Range(0, Level1Data.Length)];
            else
                wordData = TutorialData[Random.Range(0, TutorialData.Length)];

            return wordData;
        }
    }
}