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
    [SerializeField] private GameObject masterChunk;

    private void Awake()
    {
        tileLookup = pixelToTileMappings.ToDictionary(p => p.pixel.Color, p => p.tile);
    }

    public void GenerateMasterChunk(Vector2Int worldSize, PixelSO[,] pixels)
    {
        GameObject childObj = Instantiate(masterChunk, Vector3.zero, Quaternion.identity).transform.GetChild(0).gameObject;
        Tilemap masterTilemap = childObj.GetComponent<Tilemap>();

        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                PixelSO pixel = pixels[x, y];
                if (tileLookup.TryGetValue(pixel.Color, out var tile))
                {
                    masterTilemap.SetTile(new Vector3Int(x - (int)(worldSize.x * 0.5f), y - (int)(worldSize.y * 0.5f), 0), tile);
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

