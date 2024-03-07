using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int maxRoomWidth = 4, maxRoomHeight = 4;
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
    [SerializeField]
    private GameObject bossPrefab;
    [SerializeField]
    private int EnemyPerRoom;
    [SerializeField]
    private List<GameObject> enemies;
    [SerializeField]
    private List<GameObject> chess;

    public List<Vector3> centerPos;
    protected override void RunProceduralGeneration()
    {
        CreateRooms();

    }

    public void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        tilemapVisualizer.Clear();
        Destroy(gateGameObject);
        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        int countRoom = roomsList.Count;

        Vector2Int firstRoomCenter = Vector2Int.zero;
        Vector2Int lastRoomCenter = Vector2Int.zero;
        for (int i = 0; i < roomsList.Count; i++)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(roomsList[i].center));
            if (i == 0)
            {
                firstRoomCenter = (Vector2Int)Vector3Int.RoundToInt(roomsList[0].center);
                //agentPrefab.transform.position = new Vector3(firstRoomCenter.x, firstRoomCenter.y, 0);
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = new Vector3(firstRoomCenter.x, firstRoomCenter.y, 0);
            }
            else if (i == roomsList.Count - 1) // Nếu đây là phòng cuối cùng
            {
                lastRoomCenter = (Vector2Int)Vector3Int.RoundToInt(roomsList[i].center); // Lưu tâm của phòng cuối cùng
                                                                                         //    gatePrefab.transform.position = new Vector3(lastRoomCenter.x, lastRoomCenter.y, 0);
                Instantiate(gatePrefab, new Vector3(lastRoomCenter.x, lastRoomCenter.y, 0), Quaternion.identity);
            }
            else if (i == roomsList.Count - 2)  // Nếu đây là phòng kế cuối
            {
                // Tính toán vị trí trung tâm của phòng

                Vector3 chessPosition = new Vector3(roomsList[i].center.x, roomsList[i].center.y, 0);
                centerPos.Add(chessPosition);

                // Instantiate enemy prefab at the calculated position (trung tâm của phòng)
                Instantiate(GetRandomChess(), chessPosition, Quaternion.identity);
            }
            else  // Nếu đây là phòng bth
            {
                for (int j = 0; j < EnemyPerRoom; j++)
                {
                    // Tính toán vị trí trung tâm của phòng
                    Vector2 randomOffset = Random.insideUnitCircle * 3f;
                    Vector3 enemyPosition = new Vector3(roomsList[i].center.x, roomsList[i].center.y, 0) + new Vector3(randomOffset.x, randomOffset.y, 0);
                    centerPos.Add(enemyPosition);

                    // Instantiate enemy prefab at the calculated position (trung tâm của phòng)
                    Instantiate(GetRandomEnemy(), enemyPosition, Quaternion.identity);
                }
            }



        }

        //Vector3 bossPosition = new Vector3(roomsList[roomsList.Count - 1].center.x, roomsList[roomsList.Count - 1].center.y, 0);


        //// Instantiate enemy prefab at the calculated position (trung tâm của phòng)
        //Instantiate(bossPrefab, bossPosition, Quaternion.identity);
        // Instantiate enemy prefab at the calculated position (trung tâm của phòng)
        //Instantiate(bossPrefab, bossPosition, Quaternion.identity);

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

        if (gateGameObject != null)
        {
            Destroy(gateGameObject);
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
    GameObject GetRandomEnemy()
    {
        int ramdomIndex = Random.Range(0, enemies.Count);
        return enemies[ramdomIndex];
    }

    GameObject GetRandomChess()
    {
        int ramdomIndex = Random.Range(0, chess.Count);
        return chess[ramdomIndex];
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



}
