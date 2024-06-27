namespace Treasure.Chests
{
    using UnityEngine;
    using Treasure.Common;
    using System.Collections.Generic;

    public class ChestBuilder : MonoBehaviour
    {
        [SerializeField] private WordDataConfiguration _wordDataConfiguration = null;
        public static ChestBuilder Instance;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        public void BuildChestFromData(Chest chest, int currentLevel)
        {
            WordData wordData = _wordDataConfiguration.RandomizeWordData(currentLevel);
            ChestData chestData = CreateChestData(GetNumberOfTries(currentLevel), wordData, GetTrapType(currentLevel));
            chest.Init(chestData);
        }

        private ChestData CreateChestData(int tries, WordData wordData, TrapType trapType)
        {
            var chestData = new ChestData();
            chestData.WordData = wordData;
            chestData.Tries = tries;
            chestData.trapType = trapType;
            return chestData;
        }

        private int GetNumberOfTries(int currentLevel)
        {
            int tries = 0;
            if(currentLevel >= 15) tries = 1;
            else if(currentLevel >= 12) tries = 2;
            else if(currentLevel >= 4) tries = 3;

            return tries;
        }

        private TrapType GetTrapType(int currentLevel)
        {
            TrapType trapType = TrapType.None;
            List<TrapType> possibleTraps = new List<TrapType>();

            if(currentLevel >= 15)
            {
                possibleTraps.Add(TrapType.Fire);
                possibleTraps.Add(TrapType.Spikes);
                possibleTraps.Add(TrapType.Arrow);
            }
            else if(currentLevel >= 12)
            {
                possibleTraps.Add(TrapType.None);
                possibleTraps.Add(TrapType.Fire);
                possibleTraps.Add(TrapType.Spikes);
            } 
            else if(currentLevel >= 8)
            {
                possibleTraps.Add(TrapType.None);
                possibleTraps.Add(TrapType.Spikes);
                possibleTraps.Add(TrapType.Arrow);
            }
            else if(currentLevel >= 4)
            {
                possibleTraps.Add(TrapType.None);
                possibleTraps.Add(TrapType.Arrow);
            }

            if(possibleTraps.Count > 0) trapType = possibleTraps[Random.Range(0, possibleTraps.Count)];

            return trapType;
        }
    }
}

