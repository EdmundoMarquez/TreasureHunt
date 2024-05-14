using UnityEngine;
using Treasure.Common;

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
            Debug.Log("Finished Dungeon");
        }
    }
}
