using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{

    [SerializeField] private List<PixelToTileMapping> pixelToTileMappings;
    private Dictionary<Color, WorldTile> tileLookup;

    [Header("Chunk Settings")]
    [SerializeField] private Vector2Int chunkSize;
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private GameObject chunksFolder;

    private void Awake()
    {
        tileLookup = pixelToTileMappings.ToDictionary(p => p.pixel.Color, p => p.tile);
    }

    public void SplitTheWorldIntoChunks(Vector2Int worldSize, PixelSO[,] pixels)
    {
        int chunkSize = 64;
        int chunksX = worldSize.x / chunkSize;
        int chunksY = worldSize.y / chunkSize;

        for (int yChunk = 0; yChunk < chunksY; yChunk++)
        {
            for (int xChunk = 0; xChunk < chunksX; xChunk++)
            {
                GameObject chunkObj = Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity, chunksFolder.transform);
                chunkObj.name = $"Chunk ({xChunk}, {yChunk})";
                GameObject childObj = chunkObj.transform.GetChild(0).gameObject;
                Tilemap masterTilemap = childObj.GetComponent<Tilemap>();

                int startX = xChunk * chunkSize;
                int startY = yChunk * chunkSize;

                for (int x = startX; x < startX + chunkSize; x++)
                {
                    for (int y = startY; y < startY + chunkSize; y++)
                    {
                        PixelSO pixel = pixels[x, y];
                        if (tileLookup.TryGetValue(pixel.Color, out var tile))
                        {
                            masterTilemap.SetTile(new Vector3Int(x - (worldSize.x / 2), y - (worldSize.y / 2), 0), tile);
                        }
                    }
                }
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

