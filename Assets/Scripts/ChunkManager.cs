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
    private Dictionary<Vector2Int, Tilemap> chunkTilemaps = new();
    public Tilemap CurrentTilemap;

    private void Awake()
    {
        tileLookup = pixelToTileMappings.ToDictionary(p => p.pixel.Color, p => p.tile);
    }

    private void Update()
    {
        if (CurrentTilemap != null)
        {
            //for (int i = -1; i <= 1; i++)
            //{
            //    Vector2Int coordinate = new Vector2Int(32 * i + CurrentTilemap.c, 32 * i);
            //    chunkTilemaps[(32 * i, 32 * i)];
            //}

            CurrentTilemap.gameObject.SetActive(true);
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
                Tilemap chunkTilemap = childObj.GetComponent<Tilemap>();

                int startX = xChunk * chunkSize.x;
                int startY = yChunk * chunkSize.y;

                for (int x = startX; x < startX + chunkSize.x; x++)
                {
                    for (int y = startY; y < startY + chunkSize.y; y++)
                    {
                        PixelSO pixel = pixels[x, y];
                        if (tileLookup.TryGetValue(pixel.Color, out var tile))
                        {
                            chunkTilemap.SetTile(new Vector3Int(x - (worldSize.x / 2), y - (worldSize.y / 2), 0), tile);
                        }

                        int centerX = startX + chunkSize.x / 2;
                        int centerY = startY + chunkSize.y / 2;

                        if (x == centerX && y == centerY)
                        {
                            Vector3Int centerTilePos = new Vector3Int(x - (worldSize.x / 2), y - (worldSize.y / 2), 0);
                            Vector3 worldCenterPos = chunkTilemap.CellToWorld(centerTilePos) + chunkTilemap.tileAnchor;
                            chunkTilemaps.Add(new Vector2Int(Mathf.FloorToInt(worldCenterPos.x), Mathf.FloorToInt(worldCenterPos.y)), chunkTilemap);

                            chunkTilemap.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        CurrentTilemap = FindClosestChunk();
    }

    private Tilemap FindClosestChunk()
    {
        int playerX = Mathf.FloorToInt(playerTrans.position.x);
        int playerY = Mathf.FloorToInt(playerTrans.position.y);
        float shortestDistance = float.MaxValue;
        Tilemap closestTileMap = null;

        foreach (var chunk in chunkTilemaps)
        {
            float distance = Vector2Int.Distance(chunk.Key, new Vector2Int(playerX, playerY));
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestTileMap = chunk.Value;
            }

        }

       return closestTileMap;
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

        foreach (var chunk in chunkTilemaps)
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

