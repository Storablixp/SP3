using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldBuilder : MonoBehaviour
{
    [SerializeField] private Tilemap worldTilemap;
    [SerializeField] private List<PixelToTileMapping> pixelToTileMappings;

    private Dictionary<Color, WorldTile> tileLookup;

    private void Awake()
    {
        tileLookup = pixelToTileMappings.ToDictionary(p => p.pixel.Color, p => p.tile);
    }

    public void BuildWorld(Vector2Int worldSize, PixelSO[,] pixels)
    {
        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                PixelSO pixel = pixels[x, y];
                if (tileLookup.TryGetValue(pixel.Color, out var tile))
                {
                    worldTilemap.SetTile(new Vector3Int(x - (int)(worldSize.x * 0.5f), y - (int)(worldSize.y * 0.5f), 0), tile);
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

