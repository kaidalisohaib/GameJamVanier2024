using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DungeonGenerator.cs
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public int smallRoomCount = 2;
    public int mediumRoomCount = 2;
    public int largeRoomCount = 1;

    private List<GameObject> spawnedRooms = new List<GameObject>();

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < smallRoomCount; i++)
        {
            SpawnRandomRoom(Room.RoomSize.Small);
        }

        for (int i = 0; i < mediumRoomCount; i++)
        {
            SpawnRandomRoom(Room.RoomSize.Medium);
        }

        for (int i = 0; i < largeRoomCount; i++)
        {
            SpawnRandomRoom(Room.RoomSize.Large);
        }
    }

    void SpawnRandomRoom(Room.RoomSize size)
    {
        GameObject randomRoomPrefab = GetRandomRoomPrefab();
        GameObject newRoom = Instantiate(randomRoomPrefab, transform.position, Quaternion.identity);
        newRoom.GetComponent<Room>().roomSize = size;
        spawnedRooms.Add(newRoom);
    }

    GameObject GetRandomRoomPrefab()
    {
        return roomPrefabs[Random.Range(0, roomPrefabs.Length)];
    }
}
