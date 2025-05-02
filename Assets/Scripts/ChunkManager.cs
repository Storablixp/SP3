using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Cinemachine;

public class ChunkManager : MonoBehaviour
{
    [Header("Player & Camera")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CinemachineConfiner2D confiner2D;
    private Transform playerTrans;
    private BoxCollider2D cameraBox;

    [Header("Chunk Settings")]
    [SerializeField] private Vector2Int chunkSize;
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private GameObject chunksFolder;
    private Dictionary<Vector2Int, ChunkInstance> chunks = new();
    private ChunkInstance currentChunk;

    //Vi använder dessa världen för att scale på ChunkPrefab är satt til (0.5f, 0.5f, 1f).
    private int halfWitdh;
    private int halfHeight;

    private void Start()
    {
        halfWitdh = chunkSize.x / 2;
        halfHeight = chunkSize.y / 2;

        cameraBox = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (currentChunk != null)
        {
            Vector3 playerWorldPos = playerTrans.position;
            Vector3Int playerCellPos = currentChunk.Tilemap.WorldToCell(playerWorldPos);

            if (!currentChunk.Bounds.Contains(playerCellPos))
            {
                var newClosetChunk = FindClosestChunk();
                UpdateCurrentChunk(newClosetChunk);
            }

        }
    }

    public void SplitTheWorldIntoChunks(Vector2Int worldSize, PixelSO[,] pixels)
    {
        int chunksX = worldSize.x / chunkSize.x;
        int chunksY = worldSize.y / chunkSize.y;

        Vector3 vectorForPlayerSpawaning = Vector3.zero;

        for (int yChunk = 0; yChunk < chunksY; yChunk++)
        {
            for (int xChunk = 0; xChunk < chunksX; xChunk++)
            {
                GameObject chunkObj = Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity, chunksFolder.transform);
                chunkObj.name = $"Chunk ({xChunk}, {yChunk})";
                GameObject childObj = chunkObj.transform.GetChild(0).gameObject;
                childObj.name = $"Tilemap ({xChunk}, {yChunk})";

                ChunkInstance chunkInstance = childObj.GetComponent<ChunkInstance>();
                Tilemap tilemap = chunkInstance.Tilemap;

                int startX = xChunk * chunkSize.x;
                int startY = yChunk * chunkSize.y;

                for (int x = startX; x < startX + chunkSize.x; x++)
                {
                    for (int y = startY; y < startY + chunkSize.y; y++)
                    {
                        PixelSO pixel = pixels[x, y];
                        if (PixelToTileConverter.TileLookup.TryGetValue(pixel.Color, out var tile))
                        {
                            tilemap.SetTile(new Vector3Int(x - (worldSize.x / 2), y - (worldSize.y / 2), 0), tile);
                        }

                        int centerX = startX + chunkSize.x / 2;
                        int centerY = startY + chunkSize.y / 2;

                        if (x == centerX && y == centerY)
                        {
                            Vector3Int centerTilePos = new Vector3Int(x - (worldSize.x / 2), y - (worldSize.y / 2), 0);
                            Vector3 worldCenterPos = tilemap.CellToWorld(centerTilePos) /*+ tilemap.tileAnchor*/;
                            chunks.Add(new Vector2Int(Mathf.FloorToInt(worldCenterPos.x), Mathf.FloorToInt(worldCenterPos.y)), chunkInstance);

                            chunkInstance.CenterPos = new Vector2Int(Mathf.FloorToInt(worldCenterPos.x), Mathf.FloorToInt(worldCenterPos.y));
                            chunkInstance.Bounds = new BoundsInt(new Vector3Int(startX - (worldSize.x / 2), startY - (worldSize.y / 2), 0),
                                                   new Vector3Int(chunkSize.x, chunkSize.y, 1)
);
                            chunkInstance.gameObject.SetActive(false);

                            if (xChunk == chunksX / 2 && yChunk == chunksY - 1)
                            {
                                vectorForPlayerSpawaning = worldCenterPos;
                            }
                        }
                    }
                }
            }
        }

        AssignNeighborChunks();
        SpawnPlayer(worldSize, vectorForPlayerSpawaning);
        UpdateCurrentChunk(FindClosestChunk());
    }

    private void AssignNeighborChunks()
    {
        foreach (var chunk in chunks)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0) continue;

                    Vector2Int posToCheck = new Vector2Int(chunk.Value.CenterPos.x + (x * halfWitdh), chunk.Value.CenterPos.y + (y * halfHeight));
                    if (chunks.TryGetValue(posToCheck, out ChunkInstance neighborChunk))
                    {
                        chunk.Value.NeighborChunks[x + 1, y + 1] = neighborChunk;
                    }
                }
            }
        }
    }

    private void SpawnPlayer(Vector2Int worldSize, Vector3 vectorForPlayerSpawaning)
    {
        Vector2 playerSpawnPoint = new Vector2(vectorForPlayerSpawaning.x, vectorForPlayerSpawaning.y + (halfHeight / 2f) + 1f);
        playerTrans = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity).transform;
        cinemachineCamera.Follow = playerTrans;
        cameraBox.size = new Vector2(worldSize.x / 2, (worldSize.y / 2) + 16);
        cameraBox.offset = new Vector2(0, 8);
        confiner2D.BoundingShape2D = cameraBox;
    }

    private void UpdateCurrentChunk(ChunkInstance closestChunk)
    {
        HashSet<ChunkInstance> newActiveChunks = new HashSet<ChunkInstance>();
        ChunkInstance previousCurrent = currentChunk;

        // Step 1: Define which chunks should be active
        newActiveChunks.Add(closestChunk);
        foreach (var neighbor in closestChunk.NeighborChunks)
        {
            if (neighbor != null)
                newActiveChunks.Add(neighbor);
        }

        // Step 2: Deactivate old current and its neighbors if they're no longer needed
        if (previousCurrent != null)
        {
            if (!newActiveChunks.Contains(previousCurrent))
                previousCurrent.gameObject.SetActive(false);

            foreach (var neighbor in previousCurrent.NeighborChunks)
            {
                if (neighbor != null && !newActiveChunks.Contains(neighbor))
                    neighbor.gameObject.SetActive(false);
            }
        }

        // Step 3: Activate new current and its neighbors
        currentChunk = closestChunk;
        foreach (var chunk in newActiveChunks)
        {
            chunk.gameObject.SetActive(true);
        }
    }

    private ChunkInstance FindClosestChunk()
    {
        int playerX = Mathf.FloorToInt(playerTrans.position.x);
        int playerY = Mathf.FloorToInt(playerTrans.position.y);
        float shortestDistance = float.MaxValue;
        ChunkInstance closestChunk = null;

        foreach (var chunk in chunks)
        {
            float distance = Vector2Int.Distance(chunk.Key, new Vector2Int(playerX, playerY));
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestChunk = chunk.Value;
            }

        }

        return closestChunk;
    }
}
