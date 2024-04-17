namespace Treasure.Swords
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SwordFactory : MonoBehaviour
    {
        public SwordData[] _swordsDatabase = null;
        private Dictionary<string, SwordData> _idToSword;
        public static SwordFactory Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            _idToSword = new Dictionary<string, SwordData>();

            foreach (var sword in _swordsDatabase)
            {
                _idToSword.Add(sword.SwordId.Value, sword);
            }
        }

        public SwordData GetSwordById(string id)
        {
            if (!_idToSword.TryGetValue(id, out var sword)) { return null; }
            return sword;
        }

    }
}