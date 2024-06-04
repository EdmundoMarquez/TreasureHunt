namespace Treasure.InfiniteMode
{
    using UnityEngine;
    using Treasure.Common;
    using Treasure.EventBus;

    public class DungeonObjective : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _objectiveSprite;
        [SerializeField] private MinimapIconDisplay _minimapIcon = null;

        private void Start()
        {
            _minimapIcon.Init(_objectiveSprite.sprite);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                if(!col.GetComponent<IPlayableCharacter>().IsActive) return;
                GetComponent<Collider2D>().enabled = false;
                EventBus<OnCompletedDungeon>.Raise(new OnCompletedDungeon());
            }
        }
    }
}
