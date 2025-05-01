using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;
    [SerializeField] private List<PixelToTileMapping> pixelToTileMappings;
    private Dictionary<Color, WorldTile> tileLookup;

    [Header("Chunk Settings")]
    [SerializeField] private Vector2Int chunkSize;
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private GameObject chunksFolder;
    private Dictionary<Vector2Int, ChunkInstance> chunks = new();
    public ChunkInstance CurrentChunk;

    private void Awake()
    {
        tileLookup = pixelToTileMappings.ToDictionary(p => p.pixel.Color, p => p.tile);
    }

    private void Update()
    {
        if (CurrentChunk != null)
        {
            CurrentChunk.gameObject.SetActive(true);

            foreach (var neighbor in CurrentChunk.NeighborChunks)
            {
                if (neighbor != null)
                {
                    neighbor.gameObject.SetActive(true);
                }
            }
        }
    }

    public void SplitTheWorldIntoChunks(Vector2Int worldSize, PixelSO[,] pixels)
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
                        if (tileLookup.TryGetValue(pixel.Color, out var tile))
                        {
                            tilemap.SetTile(new Vector3Int(x - (worldSize.x / 2), y - (worldSize.y / 2), 0), tile);
                        }

                        int centerX = startX + chunkSize.x / 2;
                        int centerY = startY + chunkSize.y / 2;

                        if (x == centerX && y == centerY)
                        {
                            Vector3Int centerTilePos = new Vector3Int(x - (worldSize.x / 2), y - (worldSize.y / 2), 0);
                            Vector3 worldCenterPos = tilemap.CellToWorld(centerTilePos) + tilemap.tileAnchor;
                            chunks.Add(new Vector2Int(Mathf.FloorToInt(worldCenterPos.x), Mathf.FloorToInt(worldCenterPos.y)), chunkInstance);

                            chunkInstance.CenterPos = new Vector2Int(Mathf.FloorToInt(worldCenterPos.x), Mathf.FloorToInt(worldCenterPos.y));
                            chunkInstance.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        foreach (var chunk in chunks)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0) continue;

                    Vector2Int posToCheck = new Vector2Int(chunk.Value.CenterPos.x + (x * 32), chunk.Value.CenterPos.y + (y * 32));
                    if (chunks.TryGetValue(posToCheck, out ChunkInstance neighborChunk))
                    {
                        chunk.Value.NeighborChunks[x + 1, y + 1] = neighborChunk;
                        chunk.Value.Ne.Add(neighborChunk);
                    }
                }
            }
        }


        CurrentChunk = FindClosestChunk();
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

    //private void Update()
    //{
    //    if (hasBeenSetUp)
    //    {
    //        FindActiveChunks();
    //    }
    //}

    private void FindActiveChunks()
    {
        int playerX = Mathf.FloorToInt(playerTrans.position.x);
        int playerY = Mathf.FloorToInt(playerTrans.position.y);

        foreach (var chunk in chunks)
        {
            if (Vector2Int.Distance(chunk.Key, new Vector2Int(playerX, playerY)) > (chunkSize.x + chunkSize.y) * 2)
            {
                chunk.Value.gameObject.SetActive(false);
            }
        }
    }

    [System.Serializable]
    public class PixelToTileMapping
    {
        public PixelSO pixel;
        public WorldTile tile;
    }
}

