using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Treasure.Player;
using Treasure.EventBus;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using Pathfinding;

public class RoomContentGenerator : MonoBehaviour, IEventReceiver<OnDungeonFloorReady>
{
    [SerializeField]
    private RoomGenerator playerRoom, defaultRoom, treasureRoom;

    List<GameObject> spawnedObjects = new List<GameObject>();

    [SerializeField]
    private GraphTest graphTest;

    public Transform itemParent;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineCamera;

    public UnityEvent RegenerateDungeon;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var item in spawnedObjects)
            {
                Destroy(item);
            }
            RegenerateDungeon?.Invoke();
        }
    }
    public void GenerateRoomContent(DungeonData dungeonData)
    {
        foreach (GameObject item in spawnedObjects)
        {
            DestroyImmediate(item);
        }
        spawnedObjects.Clear();

        SelectPlayerSpawnPoint(dungeonData);
        SelectDungeonSpawnPoints(dungeonData);

        foreach (GameObject item in spawnedObjects)
        {
            if (item != null)
                item.transform.SetParent(itemParent, false);
        }
    }

    private void SelectPlayerSpawnPoint(DungeonData dungeonData)
    {
        int randomRoomIndex = Random.Range(0, dungeonData.roomsDictionary.Count);
        Vector2Int playerSpawnPoint = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        graphTest.RunDijkstraAlgorithm(playerSpawnPoint, dungeonData.floorPositions);

        Vector2Int roomIndex = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerSpawnPoint,
            dungeonData.roomsDictionary.Values.ElementAt(randomRoomIndex),
            dungeonData.GetRoomFloorWithoutCorridors(roomIndex)
            );

        InitializePlayer(placedPrefabs[placedPrefabs.Count - 1].transform);

        spawnedObjects.AddRange(placedPrefabs);

        dungeonData.roomsDictionary.Remove(playerSpawnPoint);
    }


    private void InitializePlayer(Transform playerTransform)
    {
        CharacterInstaller playerInstaller = playerTransform.GetComponent<CharacterInstaller>();
        playerInstaller.Init(cinemachineCamera);
    }

    private void SelectDungeonSpawnPoints(DungeonData dungeonData)
    {
        foreach (KeyValuePair<Vector2Int, HashSet<Vector2Int>> roomData in dungeonData.roomsDictionary)
        {
            if (Random.value > 0.5f) //50% chance of getting default room
            {
                spawnedObjects.AddRange(
                    defaultRoom.ProcessRoom(
                        roomData.Key,
                        roomData.Value,
                        dungeonData.GetRoomFloorWithoutCorridors(roomData.Key)
                        )
                );
                continue;
            }

            spawnedObjects.AddRange(
                    treasureRoom.ProcessRoom(
                        roomData.Key,
                        roomData.Value,
                        dungeonData.GetRoomFloorWithoutCorridors(roomData.Key)
                        )
                );
        }
    }

    public void OnEvent(OnDungeonFloorReady e)
    {
        GenerateRoomContent(e.dungeonData);
    }

    private void OnEnable()
    {
        EventBus<OnDungeonFloorReady>.Register(this);
    }

    private void OnDisable()
    {
        EventBus<OnDungeonFloorReady>.UnRegister(this);
    }
}
