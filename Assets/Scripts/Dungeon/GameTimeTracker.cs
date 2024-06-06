namespace Treasure.Dungeon
{
    using UnityEngine;

    public class GameTimeTracker : MonoBehaviour
    {
        private float _totalTimePlaying;
        public float GameTime => Mathf.RoundToInt(_totalTimePlaying);

        private void Update()
        {
            _totalTimePlaying += Time.deltaTime;
        }

    }

}