using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Cinemachine;
using System.Collections;

public class ChunkAndPlayerGenerator : MonoBehaviour
{
    private ChunkManager chunkManager;

    [Header("Chunk Settings")]
    [SerializeField] private Vector2Int chunkSize;
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private GameObject chunksFolder;
    private Dictionary<Vector2Int, ChunkInstance> chunks = new();
    private ChunkInstance currentChunk;

    [Header("Player Camera")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CinemachineConfiner2D confiner2D;
    private BoxCollider2D cameraBox;

    [Header("Player Spawning")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject playerPrefab;
    private Transform playerTrans;
    private Vector2 playerSpawnPosition;


    //Jag använder dessa världen för att scale på ChunkPrefab är satt til (0.5f, 0.5f, 1f).
    private int halfWitdh;
    private int halfHeight;

    private void Awake()
    {
        chunkManager = GetComponent<ChunkManager>();
    }

    private void Start()
    {
        halfWitdh = chunkSize.x / 2;
        halfHeight = chunkSize.y / 2;

        cameraBox = GetComponent<BoxCollider2D>();
    }

    public IEnumerator SpawnChunksAndPlayer(Vector2Int worldSize, PixelSO[,] pixels)
    {
        yield return CreateChunks(worldSize, pixels);
        yield return FindSpawnPoint();
        yield return AssignNeighborChunks();
        yield return SpawnPlayer(worldSize);

        chunkManager.SetUp(playerTrans, currentChunk, chunks);
    }

    private IEnumerator CreateChunks(Vector2Int worldSize, PixelSO[,] pixels)
    {
        int chunksX = worldSize.x / chunkSize.x;
        int chunksY = worldSize.y / chunkSize.y;

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

                            if (xChunk != chunksX / 2)
                            {
                                chunkInstance.gameObject.SetActive(false);
                            }

                            if (xChunk == chunksX / 2 && yChunk == chunksY - 1)
                            {
                                playerSpawnPosition = new Vector3(worldCenterPos.x, worldCenterPos.y + (halfHeight / 2), 0);
                            }
                        }
                    }
                }
            }
        }

        yield return null;
    }
    private IEnumerator AssignNeighborChunks()
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

        yield return null;
    }
    private IEnumerator FindSpawnPoint()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerSpawnPosition, Vector2.down, Mathf.Infinity, groundMask);
        if (hit.collider != null)
        {
            playerSpawnPosition = hit.point;
            currentChunk = hit.collider.gameObject.GetComponent<ChunkInstance>();
        }

        foreach (var chunk in chunks)
        {
            if (chunk.Value == currentChunk) continue;
            chunk.Value.gameObject.SetActive(false);
        }

        yield return null;
    }
    private IEnumerator SpawnPlayer(Vector2Int worldSize)
    {
        playerTrans = Instantiate(playerPrefab, new Vector2(playerSpawnPosition.x, playerSpawnPosition.y + 0.25f), Quaternion.identity).transform;
        cinemachineCamera.Follow = playerTrans;
        cameraBox.size = new Vector2(worldSize.x / 2, (worldSize.y / 2) + 16);
        cameraBox.offset = new Vector2(0, 8);
        confiner2D.BoundingShape2D = cameraBox;

        yield return null;
    }
}
