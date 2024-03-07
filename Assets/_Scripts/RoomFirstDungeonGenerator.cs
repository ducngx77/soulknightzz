using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;
    [SerializeField]
    private GameObject gatePrefab;
    private GameObject gateGameObject;
    [SerializeField]
    private GameObject agentPrefab;
    [SerializeField]
    private GameObject enemyPrefab;
    public List<Vector3> centerPos;
    [SerializeField] float minSpawnTime;
    [SerializeField] float maxSpawnTime;
    private float timeUtilSpawn;
    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    public void CreateRooms()
    {
        // Xóa các màn hình và cổng cũ
        tilemapVisualizer.Clear();
        Destroy(gateGameObject);

        // Tạo một màn chơi mới
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        // Tạo phòng và hành lang
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        Vector2Int lastRoomCenter = Vector2Int.zero;
        for (int i = 0; i < roomsList.Count; i++)
        {
            var room = roomsList[i];
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
            if (i == roomsList.Count - 1)
            {
                lastRoomCenter = (Vector2Int)Vector3Int.RoundToInt(room.center);
            }
        }

        // Kết nối các phòng với nhau
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        // Vẽ các ô sàn và tường
        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

        Vector2Int firstRoomCenter = Vector2Int.zero; // Lưu tâm của phòng đầu tiên
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
            if (firstRoomCenter == Vector2Int.zero) // Nếu chưa gán tâm của phòng đầu tiên
            {
                firstRoomCenter = (Vector2Int)Vector3Int.RoundToInt(room.center); // Gán tâm của phòng đầu tiên
            }
        }
        // Đặt người chơi tại tâm của phòng đầu tiên
        agentPrefab.transform.position = new Vector3(firstRoomCenter.x, firstRoomCenter.y, 0);


        // Đặt cổng tại tâm của phòng cuối cùng
        // gateGameObject = Instantiate(gatePrefab, new Vector3(lastRoomCenter.x, lastRoomCenter.y, 0), Quaternion.identity);
        gatePrefab.transform.position = new Vector3(lastRoomCenter.x, lastRoomCenter.y, 0);

        foreach (var room in roomsList)
        {
            // Calculate the position for enemy instantiation (you can adjust this according to your room layout)
            Vector3 enemyPosition = new Vector3(room.center.x, room.center.y, 0);
            centerPos.Add(enemyPosition);
            // Instantiate enemy prefab at the calculated position
            
        }
    }


    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
    void Update()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        List<Vector3> pos = new List<Vector3>();
        timeUtilSpawn -= Time.deltaTime;



        if (timeUtilSpawn <= 0f)
        {
            foreach (var room in centerPos)
            {
                // Calculate the position for enemy instantiation (you can adjust this according to your room layout)
                Vector3 enemyPosition = new Vector3(room.x, room.y, 0);
                pos.Add(enemyPosition);
                // Instantiate enemy prefab at the calculated position
                Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
            }


            SetTimeUtilSpawn();
        }
    }

    private void Awake()
    {
        SetTimeUtilSpawn();
    }

    private void SetTimeUtilSpawn()
    {
        timeUtilSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
