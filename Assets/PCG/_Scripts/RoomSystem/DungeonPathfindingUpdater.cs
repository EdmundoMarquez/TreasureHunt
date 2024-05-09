using UnityEngine;
using Pathfinding;
using System.Collections;
using UnityEngine.Tilemaps;

public class DungeonPathfindingUpdater : MonoBehaviour
{
    [SerializeField] private Tilemap dungeonFloor;
    private Coroutine GenerateCoroutine;

    public void GenerateNavMesh()
    {
        if(GenerateCoroutine != null)
        {
            StopCoroutine(GenerateCoroutine);
        }
        GenerateCoroutine = StartCoroutine(GenerateNavMeshTimer());
    }

    private IEnumerator GenerateNavMeshTimer()
    {
        yield return new WaitForSeconds(1f);
        var gg = AstarPath.active.data.gridGraph;
        gg.center = dungeonFloor.cellBounds.center;

        Vector2Int tilemapSize = (Vector2Int) dungeonFloor.cellBounds.size;
        gg.SetDimensions(tilemapSize.x, tilemapSize.y, 1f);

        gg.rotation = new Vector3(90,0,0);
        gg.collision.use2D = true;

        AstarPath.active.Scan();


    }
}
